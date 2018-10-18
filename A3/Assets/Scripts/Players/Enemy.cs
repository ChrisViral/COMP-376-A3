using System.Collections;
using UnityEngine;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Enemy ship
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(AudioSource))]
    public class Enemy : Ship
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Shooting delay")]
        private float minDelay;
        [SerializeField]
        private float maxDelay;
        [SerializeField]
        private float range;

        //Private fields
        private Player player;
        #endregion

        #region Functions
        private IEnumerator Start()
        {
            //Get player instance;
            this.player = GameLogic.CurrentGame.player;

            //If the Enemy is allowed to shoot
            if (this.canShoot)
            {
                //Wait in the given delay range then start shooting
                this.canShoot = false;
                yield return new WaitForSeconds(Random.Range(this.minDelay, this.maxDelay));
                this.canShoot = true;
            }
        }

        protected override void OnUpdate()
        {
            //Normal mode
            if (!GameLogic.IsHard) { base.OnUpdate(); }
            else if (this.player != null)
            {
                //Fire when close on the X axis in front of the player
                if (Mathf.Abs(this.player.transform.position.x - this.transform.position.x) <= this.range && this.player.transform.position.z < this.transform.position.z)
                {
                    FireGun();
                }
            }
        }
        #endregion
    }
}