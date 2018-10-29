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
        private float heightVariation;

        //Private fields
        private float diff;
        private int killed, destroyed;
        #endregion

        #region Properties
        /// <summary>
        /// Amount of enemies in the wave
        /// </summary>
        public abstract int Count { get; }
        #endregion
        
        #region Methods
        /// <summary>
        /// Call this to indicate a part of the wave has been killed
        /// </summary>
        public void OnKilled() => this.killed++;

        /// <summary>
        /// Call this to indicate a part of the wave has been destroyed
        /// </summary>
        public void OnDestroyed()
        {
            if (++this.destroyed == this.Count)
            {
                //If they have also all been killed, send a completion message
                GameLogic.CurrentGame.WaveDestroyed(this.killed == this.Count);
                //Destroy only this script
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Spawns one enemy at a given spawn location
        /// </summary>
        /// <param name="offset">The offset from the regular spawn location</param>
        /// <returns>The created Enemy</returns>
        protected Enemy SpawnEnemy(Vector3 offset = new Vector3())
        {
            Enemy spawned = Instantiate(this.enemy, this.spawnLocation + offset + (Vector3.up * this.diff), Quaternion.identity);
            spawned.Spawner = this;
            return spawned;
        }

        /// <summary>
        /// Spawns the enemy waves
        /// </summary>
        protected override IEnumerator<YieldInstruction> SpawnWave()
        {
            //Get spawn height difference
            this.diff = Random.Range(-this.heightVariation, this.heightVariation);

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