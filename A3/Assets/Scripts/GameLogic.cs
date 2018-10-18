using System.Text;
using SpaceShooter.Scenes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    /// <summary>
    /// Game Scenes
    /// </summary>
    public enum GameScenes
    {
        MENU = 0,
        GAME = 1
    }

    /// <summary>
    /// Game difficulties
    /// </summary>
    public enum GameMode
    {
        NONE,
        NORMAL,
        HARD
    }

    /// <summary>
    /// Game logic controller
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(AudioSource))]
    public sealed class GameLogic : Singleton<GameLogic>
    {
        #region Events
        /// <summary>
        /// OnPause Delegate
        /// </summary>
        /// <param name="state">The current state of the game (paused or not)</param>
        public delegate void PauseDelegate(bool paused);

        /// <summary>
        /// On game pause event
        /// </summary>
        public static event PauseDelegate OnPause;
        #endregion

        #region Static Properties
        /// <summary>
        /// Currently loaded game scene
        /// </summary>
        public static GameScenes CurrentScene { get; private set; }

        /// <summary>
        /// Curent GameMode
        /// </summary>
        public static GameMode Mode { get; internal set; } = GameMode.NONE;

        /// <summary>
        /// If currently in Hard mode
        /// </summary>
        public static bool IsHard => Mode == GameMode.HARD;

        private static bool isPaused;
        /// <summary>
        /// If the game is currently paused
        /// </summary>
        public static bool IsPaused
        {
            get { return isPaused; }
            internal set
            {
                //Check if the value has changed
                if (isPaused != value)
                {
                    //Set value and stop Unity time
                    isPaused = value;
                    Time.timeScale = isPaused ? 0f : 1f;

                    Instance.Log($"Game {(isPaused ? "paused" : "unpaused")}");

                    //Fire pause event
                    OnPause?.Invoke(isPaused);
                }
            }
        }

        /// <summary>
        /// Current ongoing Game
        /// </summary>
        public static Game CurrentGame { get; private set; }
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField, Header("Music")]
        private AudioClip menuMusic;
        [SerializeField]
        private AudioClip gameMusic;

        //Private fields
        private AudioSource source;
        #endregion

        #region Static methods
        /// <summary>
        /// Loads a given scene
        /// </summary>
        /// <param name="scene">Scene to load</param>
        internal static void LoadScene(GameScenes scene) => SceneManager.LoadScene((int)scene);

        /// <summary>
        /// Quits the game
        /// </summary>
        internal static void Quit()
        {
            //Exit depending on current situation
            Instance.Log("Exiting game...");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Converts an ALL_CAPS string to CamelCase
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns>The CamelCase version of <paramref name="s"/></returns>
        private static string ToCamelCase(string s)
        {
            //Setup Stringbuilder
            bool upper = true;
            StringBuilder sb = new StringBuilder(s.Length);

            //Loop through reference string
            foreach (char c in s)
            {
                //Add characters correctly
                if (c == '_') { upper = true; }
                else if (upper)
                {
                    sb.Append(c);
                    upper = false;
                }
                else { sb.Append(char.ToLower(c)); }
            }
            //Return the final string
            return sb.ToString();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Game scene loaded event
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="mode">Load mode</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameScenes loadedScene = (GameScenes)scene.buildIndex;
            switch (loadedScene)
            {
                    case GameScenes.MENU:
                        CurrentGame = null;
                        Mode = GameMode.NONE;
                        this.source.clip = this.menuMusic;
                        this.source.Play();
                        break;

                    case GameScenes.GAME:
                        CurrentGame = FindObjectOfType<Game>();
                        if (CurrentScene != GameScenes.GAME)
                        {
                            this.source.clip = this.gameMusic;
                            this.source.Play();
                        }
                        break;
            }
            this.Log($"Scene loaded - {ToCamelCase(loadedScene.ToString())}");
            CurrentScene = loadedScene;
        }
        #endregion

        #region Functions
        protected override void OnAwake()
        {
            //Opening message
            this.Log("Game started");

            //Add scene load event
            SceneManager.sceneLoaded += OnSceneLoaded;

            //Setup audio
            this.source = GetComponent<AudioSource>();
            this.source.loop = true;
        }

        private void Update()
        {
            //Pauses the game
            if (!IsPaused && CurrentScene == GameScenes.GAME && Input.GetKeyDown(KeyCode.Escape) && !CurrentGame.GameEnded)
            {
                IsPaused = true;
            }
        }

        //Make sure to remove event
        private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;
        #endregion
    }
}
