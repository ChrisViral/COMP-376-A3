using PlanetaryEscape.Physics;
using UnityEngine;

namespace PlanetaryEscape.Players
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
        [SerializeField, Header("Death")]
        protected GameObject explosion;
        [SerializeField]
        protected GameObject smoke;
        [SerializeField]
        private Transform[] engines;
        [SerializeField]
        private AudioClip disabled;
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

        #region Properties
        /// <summary>
        /// If the ship is freefalling to it's death
        /// </summary>
        public bool Controllable { get; set; } = true;
        #endregion

        #region Methods
        /// <summary>
        /// Kills the ship and sends it into freefall
        /// </summary>
        /// <returns>True if the ship has been sent into freefall, false otherwise</returns>
        public virtual bool Die()
        {
            //Destroy object and create explosion
            if (this.Controllable)
            {
                this.Controllable = false;
                this.Rigidbody.useGravity = true;

                //Replace engines by smoke
                foreach (Transform engine in this.engines)
                {
                    Instantiate(this.smoke, engine.position, Quaternion.identity, engine.parent);
                    engine.gameObject.SetActive(false);
                }
                this.source.PlayOneShot(this.disabled, 1f);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Explodes this ship
        /// </summary>
        public virtual void Explode()
        {
            Destroy(this.gameObject);
            Instantiate(this.explosion, this.transform.position, Quaternion.identity);
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
        
        protected override void OnUpdate()
        {
            //Fires the gun as soon as possible
            if (this.Controllable) { FireGun(); }
        }

        protected override void OnFixedUpdate()
        {
            //Side tilt
            this.Rigidbody.rotation = Quaternion.Euler(0f, 0f, this.Rigidbody.velocity.x * -this.tilt);
        }
        #endregion
    }
}