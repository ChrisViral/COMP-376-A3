using System.Collections.Generic;
using PlanetaryEscape.Physics;
using UnityEngine;

namespace PlanetaryEscape.Waves
{
    /// <summary>
    /// Sinusoidal movement enemy wave generator
    /// </summary>
    public class SidewaysController : EnemyWaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private int count;
        #endregion
        
        #region Properties
        /// <summary>
        /// Amount of enemies in the wave
        /// </summary>
        public override int Count => this.count;
        #endregion

        #region Methods
        protected override IEnumerator<YieldInstruction> Spawner()
        {
            //Randomly change starting side
            if (Random.value > 0.5f) { this.spawnLocation.x *= -1f; }

            //Spawn first enemy
            SpawnEnemy().GetComponent<SideMovement>().StopWait = this.count - 1;

            //Spawn wave
            for (int i = 2; i <= this.count; i++)
            {
                //Wait a before spawning next enemy, and add a sin
                yield return new WaitForSeconds(this.interval);
                SpawnEnemy().GetComponent<SideMovement>().StopWait = this.count - i;
            }
        }
        #endregion
    }
}