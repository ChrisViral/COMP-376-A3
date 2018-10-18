using System.Collections.Generic;
using SpaceShooter.Physics;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// Sinusoidal movement enemy wave generator
    /// </summary>
    public class SinusoidalController : EnemyWaveController
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
            //Spawn first enemy
            SpawnEnemy().AddComponent<SinusoidalMovement>();

            //Spawn wave
            for (int i = 1; i < this.count; i++)
            {
                //Wait a before spawning next enemy, and add a sin
                yield return new WaitForSeconds(this.interval);
                SpawnEnemy().AddComponent<SinusoidalMovement>();
            }
        }
        #endregion
    }
}