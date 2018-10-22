using PlanetaryEscape.Extensions;
using PlanetaryEscape.Physics;
using PlanetaryEscape.Utils;
using UnityEngine;
using Bounds = PlanetaryEscape.Utils.Bounds;
using Physic = UnityEngine.Physics;

namespace PlanetaryEscape.Players
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
        private float acceleration;
        [SerializeField]
        private Shield shield;
        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private RectTransform crosshairCanvas, crosshair;
        [SerializeField]
        private LayerMask enemyLayer;
        [SerializeField]
        private Animator[] lives;
        [SerializeField, Header("Powerup")]
        private AudioClip powerupSound;
        [SerializeField]
        private float powerupVolume;
        [SerializeField, Header("Cheats")]
        private bool invulnerable;
        [SerializeField, Range(0, MAX_LEVEL)]
        private int startLevel;

        //Private fields
        private Rect screen;
        #endregion

        #region Properties
        /// <summary>
        /// Current level of the player
        /// </summary>
        public int Level { get; private set; }
       
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
                this.Rigidbody.drag = 0f;
                base.Die();
                //Notify for the game to end
                //GameLogic.CurrentGame.EndGame();
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
        private void Start()
        {
            this.lives[0].SetTrigger("Toggle");
            this.Level = this.startLevel;
            this.screen = this.crosshairCanvas.rect;
            this.shield.gameObject.SetActive(true);
        }

        protected override void OnUpdate()
        {
            //If fire is pressed and enough time has elapsed since last fire, spawn a new shot
            if (this.Controllable)
            {
                if (Input.GetButton("Fire")) { FireGun(); }
                
                Vector3 screenPosition;
                if (Physic.Raycast(this.gun.position, this.gun.forward, out RaycastHit hit, 90f, this.enemyLayer, QueryTriggerInteraction.Collide))
                {
                    screenPosition = hit.point;
                }
                else
                {
                    screenPosition = this.transform.position;
                    screenPosition.z += 90f;
                }

                screenPosition = this.playerCamera.WorldToViewportPoint(screenPosition);
                this.crosshair.anchoredPosition = new Vector2(screenPosition.x * this.screen.width, screenPosition.y  * this.screen.height);
            }
            
        }
        
        protected override void OnFixedUpdate()
        {
            if (this.Controllable)
            {
                //Movement speed
                this.Rigidbody.AddForce(new Vector3(Input.GetAxis("Horizontal") * this.acceleration, Input.GetAxis("Vertical") * this.acceleration));
                this.Rigidbody.velocity.ClampTo(this.speed);

                //Limit to game bounds
                this.Rigidbody.position = this.gameLimits.BoundVector(this.Rigidbody.position);
            }

            //Call Ship.OnFixedUpdate()
            base.OnFixedUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            //If the shield is inactive and hit, damage player
            if (!this.invulnerable && this.Controllable && !this.shield.Active)
            {
                switch (other.tag)
                {
                    case "Projectile_Enemy":
                        if (other.GetComponent<Bolt>().Active) { Die(); }
                        break;

                    case "Enemy":
                        if (!Die()) { other.GetComponent<Enemy>().Die(); }
                        break;
                }
            }
        }
        #endregion
    }
}
