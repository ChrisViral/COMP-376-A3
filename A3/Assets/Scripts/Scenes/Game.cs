using System.Collections.Generic;
using System.IO;
using System.Text;
using SpaceShooter.Physics;
using SpaceShooter.Players;
using SpaceShooter.UI;
using SpaceShooter.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Scenes
{
    /// <summary>
    /// Gameplay flow controller
    /// </summary>
    [DisallowMultipleComponent]
    public class Game : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Gameplay")]
        internal Player player;
        [SerializeField]
        private AccelerationMovement background;
        [SerializeField]
        private float endGameWait;
        [SerializeField, Header("UI")]
        private GameObject pausePanel;
        [SerializeField]
        private Text scoreLabel, gameoverLabel;
        [SerializeField]
        private Animator uiAnimator;
        [SerializeField]
        private float bossUISpeed, endUISpeed;
        [SerializeField]
        private GameObject nameField;
        [SerializeField]
        private bool startPaused;
        [SerializeField, Header("Enemy waves")]
        private int waves;
        [SerializeField, Tooltip("Asteroid spawner")]
        private GameObject asteroids;
        [SerializeField, Tooltip("Enemy spawners")]
        private GameObject[] enemies;
        [SerializeField, Header("Powerup")]
        private GameObject powerup;
        [SerializeField]
        private Vector3 powerupSpawn;
        [SerializeField, Header("Boss fight")]
        private GameObject boss;
        [SerializeField]
        private Vector3 bossSpawn;
        [SerializeField]
        private float bossDelay;
        [SerializeField, Tooltip("Boss health bar")]
        internal Progressbar bossProgressbar;
        
        //Private fields
        private bool bossFight;
        private WaveController asteroidController, enemyController;
        #endregion

        #region Properties
        /// <summary>
        /// If the Game has been ended (either winning or losing)
        /// </summary>
        public bool GameEnded { get; private set; }

        private int score;
        /// <summary>
        /// Score of this game
        /// </summary>
        public int Score
        {
            get { return this.score; }
            set
            {
                this.score = value;
                this.scoreLabel.text = $"Score: {this.score}";
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ends the game cycle
        /// </summary>
        public void EndGame(bool won = false)
        {
            //End game
            if (this.asteroidController) { Destroy(this.asteroidController.gameObject); }
            if (this.enemyController) { Destroy(this.enemyController.gameObject); }
            this.GameEnded = true;
            this.player.Controllable = false;

            //Start the transition Coroutine
            StartCoroutine(won ? WinTransition() : LoseTransition());
        }

        /// <summary>
        /// Winning endgame coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator<YieldInstruction> WinTransition()
        {
            //Write password file in hard mode
            if (GameLogic.IsHard ) { File.WriteAllText(Path.Combine(Application.dataPath, @"..\Password.txt"), "Emilie sucks.", Encoding.ASCII); }

            //Stop player movement
            this.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //Lock boss progressbar

            //Wait for the endgame
            yield return new WaitForSeconds(this.endGameWait);

            //Update UI
            this.gameoverLabel.text = "Congratulations!";

            //Fade out screen
            this.background.StartMovement(AccelerationMovement.MovementMode.ACCELERATE);
            this.player.GetComponent<AccelerationMovement>().StartMovement(AccelerationMovement.MovementMode.ACCELERATE);
            this.uiAnimator.SetTrigger("End");
        }

        /// <summary>
        /// Losing endgame coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator<YieldInstruction> LoseTransition()
        {
            //Update UI and fade
            this.nameField.SetActive(false);
            this.uiAnimator.SetFloat("EndSpeed", this.endUISpeed * 2f);
            this.uiAnimator.SetTrigger("End");
            yield return new WaitForSeconds(2f / this.endUISpeed);

            //Destroy the boss if it exists after the transition
            Boss b = FindObjectOfType<Boss>();
            if (b != null)
            {
                Destroy(b.gameObject);
            }
        }

        /// <summary>
        /// Starts a random Enemy wave controller
        /// </summary>
        private void StartRandomController()
        {
            if (this.waves-- > 0)
            {
                this.enemyController = Instantiate(this.enemies[Random.Range(0, this.enemies.Length)]).GetComponent<WaveController>();
                this.enemyController.StartWave();
            }
            else { StartCoroutine(StartBossFight()); }
        }

        /// <summary>
        /// Stops incoming waves and starts the boss fight
        /// </summary>
        private IEnumerator<YieldInstruction> StartBossFight()
        {
            this.bossFight = true;
            if (!GameLogic.IsHard)
            {
                this.asteroidController.StopWave();
                Destroy(this.asteroidController.gameObject);
            }
            yield return new WaitForSeconds(this.bossDelay);
            
            this.uiAnimator.SetTrigger("Boss");
            Instantiate(this.boss, this.bossSpawn, Quaternion.identity);
        }

        /// <summary>
        /// Full enemy wave destroyed event
        /// </summary>
        public void WaveDestroyed()
        {
            this.Score *= 2;

            if (this.player.Level < Player.MAX_LEVEL)
            {
                Instantiate(this.powerup, this.powerupSpawn, Quaternion.identity);
                this.Log("A powerup has been created!");
            }
        }

        /// <summary>
        /// Pause event
        /// </summary>
        /// <param name="state">Current game pause state</param>
        public void OnPause(bool state) => this.pausePanel.SetActive(state);

        /// <summary>
        /// Resume button press
        /// </summary>
        public void OnResume() => GameLogic.IsPaused = false;

        /// <summary>
        /// Return to menu button press
        /// </summary>
        public void OnReturnToMenu()
        {
            GameLogic.IsPaused = false;
            GameLogic.LoadScene(GameScenes.MENU);
        }

        /// <summary>
        /// Restarts a new game
        /// </summary>
        public void OnRestart()
        {
            GameLogic.IsPaused = false;
            GameLogic.LoadScene(GameScenes.GAME);
        }
        #endregion

        #region Functions
        //Add OnPause listener
        private void Awake() => GameLogic.OnPause += OnPause;

        private void Start()
        {
            //Start paused
            GameLogic.IsPaused = this.startPaused;

            //Set important stuff
            this.uiAnimator.SetFloat("BossSpeed", this.bossUISpeed);
            this.uiAnimator.SetFloat("EndSpeed", this.endUISpeed);
            this.background.StartMovement(AccelerationMovement.MovementMode.APPROACH);
            this.asteroidController = Instantiate(this.asteroids).GetComponent<AsteroidWaveController>();
            this.asteroidController.StartWave();
            StartRandomController();
        }

        private void Update()
        {
            if (!this.bossFight && !this.enemyController.IsRunning)
            {
                    StartRandomController();
            }
        }

        //Remove the OnPause event
        private void OnDestroy() => GameLogic.OnPause -= OnPause;
        #endregion
    }
}