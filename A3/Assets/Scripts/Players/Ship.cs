using SpaceShooter.Physics;
using UnityEngine;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Ship base class
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public abstract class Ship : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Movement")]
        protected float speed;
        [SerializeField]
        protected float tilt;
        [SerializeField]
        protected GameObject explosion;
        [SerializeField, Header("Fire")]
        protected GameObject bolt;
        [SerializeField]
        protected AudioClip boltSound;
        [SerializeField]
        protected float shotVolume, fireRate;
        [SerializeField]
        protected Transform gun;
        public bool canShoot = true;
        
        //Private fields
        protected AudioSource source;
        protected float nextFire;
        #endregion

        #region Methods
        /// <summary>
        /// Kills the player
        /// </summary>
        public virtual bool Die()
        {
            //Destroy object and create explosion
            Destroy(this.gameObject);
            Instantiate(this.explosion, this.transform.position, Quaternion.identity);
            return true;
        }

        /// <summary>
        /// Fires the ship's weapon if possible
        /// </summary>
        /// <returns>True if the weapon has fired, false otherwise</returns>
        public bool FireGun()
        {
            if (this.canShoot && Time.time > this.nextFire)
            {
                //Fire the weapon
                this.nextFire = Time.time + this.fireRate;
                Fire();
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Fires the ship's weapon
        /// </summary>
        protected virtual void Fire()
        {
            Instantiate(this.bolt, this.gun.position, Quaternion.identity);
            this.source.PlayOneShot(this.boltSound, this.shotVolume);
        }
        #endregion

        #region Functions
        protected override void OnAwake()
        {
            //Call base Awake()
            base.OnAwake();
            //Get the audio source
            this.source = GetComponent<AudioSource>();
        }

        //Fires the gun as soon as possible
        protected override void OnUpdate() => FireGun();

        protected override void OnFixedUpdate()
        {
            //Side tilt
            this.rigidbody.rotation = Quaternion.Euler(0f, 0f, this.rigidbody.velocity.x * -this.tilt);
        }
        #endregion
    }
}
