using System.Collections;
using PlanetaryEscape.Physics;
using PlanetaryEscape.Waves;
using UnityEngine;

namespace PlanetaryEscape.Players
{
    /// <summary>
    /// Enemy ship
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(AudioSource))]
    public class Enemy : Ship
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Points")]
        private int score;
        [SerializeField, Header("Shooting delay")]
        private float minDelay;
        [SerializeField]
        private float maxDelay;
        #endregion

        #region Properties
        /// <summary>
        /// The WaveListener this object is associated to this enemy
        /// </summary>
        public EnemyWaveController Spawner { get; internal set; }
        #endregion

        #region Methods
        /// <summary>
        /// Kills the enemy and sends it into freefall
        /// </summary>
        /// <returns>True if the ship has been sent into freefall, false otherwise</returns>
        public override bool Die()
        {
            //Check if the enemy does die
            if (base.Die())
            {
                //Add score
                GameLogic.CurrentGame.Score += this.score;
                if (this.Spawner != null) { this.Spawner.OnKilled(); }
                GetComponent<Animator>().enabled = false;
                return true;
            }

            return false;
        }
        #endregion

        #region Functions
        private IEnumerator Start()
        {
            //If the Enemy is allowed to shoot
            if (this.canShoot)
            {
                //Wait in the given delay range then start shooting
                this.canShoot = false;
                yield return new WaitForSeconds(Random.Range(this.minDelay, this.maxDelay));
                this.canShoot = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //If hit by a projectile, die
            if (other.CompareTag("Projectile") && other.GetComponent<Bolt>().Active)
            {
                Die();
            }
        }

        private void OnDestroy()
        {
            //Let the listener know the object has been destroyed if any is present
            if (this.Spawner != null) { this.Spawner.OnDestroyed(); }
        }
        #endregion
    }
}