using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// Enemy wave controller class
    /// </summary>
    public abstract class WaveController : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        protected float delay;
        [SerializeField]
        protected Vector3 spawn;

        //private fields
        private Coroutine spawnRoutine;
        #endregion
        
        #region Properties
        /// <summary>
        /// True when the wave is currently being generated
        /// </summary>
        public bool IsRunning { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the wave generation coroutine
        /// </summary>
        public void StartWave()
        {
            //Start the coroutine if it isn't running
            if (!this.IsRunning)
            {
                this.spawnRoutine = StartCoroutine(Spawn());
            }
        }

        /// <summary>
        /// Stops the wave generation coroutine
        /// </summary>
        public void StopWave()
        {
            //Stop the coroutine if it is running
            if (this.IsRunning)
            {
                this.IsRunning = false;
                StopCoroutine(this.spawnRoutine);
            }
        }

        /// <summary>
        /// IsRunning wrapper around the actual Coroutine
        /// </summary>
        private IEnumerator<YieldInstruction> Spawn()
        {
            //Set running flag to true
            this.IsRunning = true;

            //Cycle through the implemented coroutine
            using (IEnumerator<YieldInstruction> spawner = SpawnWave())
            {
                while (spawner.MoveNext())
                {
                    yield return spawner.Current;
                }
            }

            //Finished running, turn off flag
            this.IsRunning = false;
        }
        #endregion

        #region Abstract methods
        /// <summary>
        /// Spawn coroutine implementation
        /// </summary>
        protected abstract IEnumerator<YieldInstruction> SpawnWave();
        #endregion
    }
}
