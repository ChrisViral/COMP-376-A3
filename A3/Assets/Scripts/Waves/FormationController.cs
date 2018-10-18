using System.Collections.Generic;
using SpaceShooter.Players;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// V-Formation enemy wave generator
    /// </summary>
    public class FormationController : EnemyWaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float spacing;
        [SerializeField]
        private int layers;
        #endregion
       
        #region Properties
        /// <summary>
        /// Amount of enemies in the wave
        /// </summary>
        public override int Count => (this.layers * 2) - 1;
        #endregion

        #region Methods
        /// <summary>
        /// Spawns enemies in a V-Formation
        /// </summary>
        protected override IEnumerator<YieldInstruction> Spawner()
        {
            //Spawn first enemy
            SpawnEnemy().GetComponent<Enemy>().canShoot = GameLogic.IsHard;

            //Spawn remaining layers
            for (int i = 1; i < this.layers; i++)
            {
                //Wait between spawns
                yield return new WaitForSeconds(this.interval);

                //Spawn both enemies side by side
                float space = this.spacing * i;
                SpawnEnemy(new Vector3(this.spawn.x - space, this.spawn.y, this.spawn.z)).GetComponent<Enemy>().canShoot = GameLogic.IsHard;
                SpawnEnemy(new Vector3(this.spawn.x + space, this.spawn.y, this.spawn.z)).GetComponent<Enemy>().canShoot = GameLogic.IsHard;
            }
        }
        #endregion
    }
}