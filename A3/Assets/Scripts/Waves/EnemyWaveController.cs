using System.Collections.Generic;
using PlanetaryEscape.Players;
using UnityEngine;

namespace PlanetaryEscape.Waves
{
    /// <summary>
    /// Sinusoidal movement enemy wave generator
    /// </summary>
    public abstract class EnemyWaveController : WaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        protected Enemy enemy;
        [SerializeField]
        protected float interval;
        [SerializeField]
        protected WaveListener listener;
        [SerializeField]
        private float heightVariation;

        //Private fields
        private float diff;
        #endregion

        #region Properties
        /// <summary>
        /// Amount of enemies in the wave
        /// </summary>
        public abstract int Count { get; }
        #endregion
        
        #region Methods
        /// <summary>
        /// Spawns one enemy at a given spawn location
        /// </summary>
        /// <param name="offset">The offset from the regular spawn location</param>
        /// <returns>The created Enemy</returns>
        protected Enemy SpawnEnemy(Vector3 offset = new Vector3())
        {
            Enemy spawned = Instantiate(this.enemy, this.spawnLocation + offset + (Vector3.up * this.diff), Quaternion.identity);
            spawned.Listener = this.listener;
            return spawned;
        }

        /// <summary>
        /// Spawns the enemy waves
        /// </summary>
        protected override IEnumerator<YieldInstruction> SpawnWave()
        {
            //Get spawn height difference
            this.diff = Random.Range(-this.heightVariation, this.heightVariation);

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