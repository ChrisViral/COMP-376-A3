using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// Sinusoidal movement enemy wave generator
    /// </summary>
    public abstract class EnemyWaveController : WaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        protected GameObject enemy;
        [SerializeField]
        protected float interval;
        [SerializeField]
        protected WaveListener listener;
        #endregion

        #region Properties
        /// <summary>
        /// Amount of enemies in the wave
        /// </summary>
        public abstract int Count { get; }
        #endregion
        
        #region Methods
        /// <summary>
        /// Spawns one enemy at the default spawn location
        /// </summary>
        /// <returns>The created GameObject</returns>
        protected GameObject SpawnEnemy() => SpawnEnemy(this.spawn);

        /// <summary>
        /// Spawns one enemy at a given spawn location
        /// </summary>
        /// <param name="position">Spawn position</param>
        /// <returns>The created GameObject</returns>
        protected GameObject SpawnEnemy(Vector3 position) => this.listener.AttachListener(Instantiate(this.enemy, position, Quaternion.identity));

        /// <summary>
        /// Spawns the enemy waves
        /// </summary>
        protected override IEnumerator<YieldInstruction> SpawnWave()
        {
            //Setup listener
            this.listener.Count = this.Count;

            //First wave delay
            yield return new WaitForSeconds(this.delay);

            //Cycle through the implemented coroutine
            using (IEnumerator<YieldInstruction> spawner = Spawner())
            {
                while (spawner.MoveNext())
                {
                    yield return spawner.Current;
                }
            }
        }
        #endregion

        #region Abstract methods
        /// <summary>
        /// Spawns the enemy in succession
        /// </summary>
        protected abstract IEnumerator<YieldInstruction> Spawner();
        #endregion
    }
}