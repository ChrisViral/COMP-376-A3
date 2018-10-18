using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Object whose behaviour an be paused by the game
    /// </summary>
    public abstract class PausableObject : MonoBehaviour
    {
        #region Virtual Methods
        /// <summary>
        /// This is called from within Update, you should override this instead of writing an Update() method
        /// </summary>
        protected virtual void OnUpdate() { }
        
        /// <summary>
        /// This is called from within FixedUpdate, you should override this instead of writing an FixedUpdate() method
        /// </summary>
        protected virtual void OnFixedUpdate() { }
        #endregion

        #region Functions
        private void Update()
        {
            //Only run when not paused
            if (!GameLogic.IsPaused)
            {
                OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            //Only run when not paused
            if (!GameLogic.IsPaused)
            {
                OnFixedUpdate();
            }
        }
        #endregion
    }
}
