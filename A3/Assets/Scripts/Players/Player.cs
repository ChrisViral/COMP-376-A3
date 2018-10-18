using SpaceShooter.Physics;
using UnityEngine;
using UnityEngine.UI;
using Bounds = SpaceShooter.Utils.Bounds;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Player object
    /// </summary>
    public class Player : Ship
    {
        #region Constants
        /// <summary>
        /// Max player level
        /// </summary>
        public const int MAX_LEVEL = 2;
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField, Header("Player")]
        private Bounds gameLimits; 
        [SerializeField]
        private Shield shield;
        [SerializeField]
        private Animator[] lives;
        [SerializeField, Header("Powerup")]
        private AudioClip powerupSound;
        [SerializeField]
        private float powerupVolume;
        [SerializeField]
        private bool invulnerable;
        #endregion

        #region Properties
        /// <summary>
        /// Current level of the player
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// If the Player can currently be controlled
        /// </summary>
        public bool Controllable { get; set; } = true;
        #endregion

        #region Methods
        /// <summary>
        /// Increments the player's level by one, up to Level 2
        /// </summary>
        /// <returns>The new player's level</returns>
        public int IncrementLevel()
        {
            if (this.Level < 2)
            {
                this.source.PlayOneShot(this.powerupSound, this.powerupVolume);
                this.lives[++this.Level].SetTrigger("Toggle");
            }
            return this.Level;
        }

        /// <summary>
        /// Decrements the players level, down to -1 (dead)
        /// </summary>
        /// <returns>The new player's level</returns>
        public int DecrementLevel()
        {
            if (this.Level >= 0)
            {
                this.lives[this.Level--].SetTrigger("Toggle");
            }
            return this.Level;
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public override bool Die()
        {
            if (this.invulnerable) { return false; }

            this.Log($"Die called at level {this.Level}");
            //Make sure life is back to zero
            if (DecrementLevel() < 0)
            {
                //Call base method
                base.Die();
                //Notify for the game to end
                GameLogic.CurrentGame.EndGame();
                return true;
            }

            this.shield.TriggerShield();
            return false;
        }

        /// <summary>
        /// Fires the weapon, according to the player's current level
        /// </summary>
        protected override void Fire()
        {
            //Normal fire
            if (this.Level == 0) { base.Fire(); }
            else
            {
                //Level 1 fire
                Vector3 pos = this.gun.position;
                pos.x -= 0.25f;
                Instantiate(this.bolt, pos, Quaternion.identity);
                pos.x += 0.5f;
                Instantiate(this.bolt, pos, Quaternion.identity);

                //Level 2 fire
                if (this.Level == 2)
                {
                    pos.x += 0.25f;
                    Instantiate(this.bolt, pos, Quaternion.Euler(0f,  25f, 0f));
                    pos.x -= 1f;
                    Instantiate(this.bolt, pos, Quaternion.Euler(0f, -25f, 0f));
                }

                //Play fire sound
                this.source.PlayOneShot(this.boltSound, this.shotVolume);
            }
        }
        #endregion

        #region Functions
        //Set first life indicator to true
        private void Start() => this.lives[0].SetTrigger("Toggle");

        protected override void OnUpdate()
        {
            //If fire is pressed and enough time has elapsed since last fire, spawn a new shot
            if (this.Controllable && Input.GetButton("Fire1"))
            {
                FireGun();
            }
        }
        
        protected override void OnFixedUpdate()
        {
            if (this.Controllable)
            {
                //Movement speed
                this.rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * this.speed, 0f, Input.GetAxis("Vertical") * this.speed);
                //Limit to game bounds
                this.rigidbody.position = this.gameLimits.BoundVector(this.rigidbody.position);
                //Call Ship.FixedUpdate()
                base.OnFixedUpdate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.invulnerable) { return; }

            //If the shield is inactive and hit, damage player
            if (this.Controllable && !this.shield.Active)
            {
                if (other.CompareTag("Projectile_Enemy") || (GameLogic.IsHard && other.CompareTag("Projectile") && other.GetComponent<Bolt>().CanHurtPlayer))
                {
                    Die();
                }
            }
        }
        #endregion
    }
}
