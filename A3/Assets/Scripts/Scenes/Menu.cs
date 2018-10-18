using System.Collections.Generic;
using SpaceShooter.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Scenes
{
    /// <summary>
    /// Menu flow controller
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Animator))]
    public class Menu : MonoBehaviour
    {
        #region Fields
        //Private fields
        private Animator menuAnimator;
        #endregion

        #region Methods
        /// <summary>
        /// Start button event
        /// </summary>
        public void OnStart() => this.menuAnimator.SetTrigger("Difficulty");

        /// <summary>
        /// Help button event
        /// </summary>
        public void OnHelp() => this.menuAnimator.SetTrigger("Help");

        /// <summary>
        /// Exit button event
        /// </summary>
        public void OnExit() => GameLogic.Quit();

        /// <summary>
        /// Normal mode button event
        /// </summary>
        public void OnNormalMode()
        {
            GameLogic.Mode = GameMode.NORMAL;
            GameLogic.LoadScene(GameScenes.GAME);
        }

        /// <summary>
        /// Hard mode button event
        /// </summary>
        public void OnHardMode()
        {
            GameLogic.Mode = GameMode.HARD;
            GameLogic.LoadScene(GameScenes.GAME);
        }
        #endregion

        #region Functions
        //Getting menu animator
        private void Awake() => this.menuAnimator = GetComponent<Animator>();
        #endregion
    }
}