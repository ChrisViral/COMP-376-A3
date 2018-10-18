using SpaceShooter.Physics;
using SpaceShooter.Utils;
using UnityEngine;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Ship shield
    /// </summary>
    [RequireComponent(typeof(Renderer), typeof(Collider))]
    public class Shield : AudioObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float duration;

        //Private fields
        private Material material;
        private float timeLeft, baseAlpha;
        #endregion

        #region Properties
        /// <summary>
        /// If this shield is currently active or not
        /// </summary>
        public bool Active { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the alpha channel of the this objects material
        /// </summary>
        /// <param name="alpha">New alpha value</param>
        private void SetAlpha(float alpha)
        {
            //Change alpha value of the material's colour
            Color c = this.material.color;
            c.a = alpha;
            this.material.color = c;
        }

        /// <summary>
        /// Turn the shield on
        /// </summary>
        public void TriggerShield()
        {
            this.Active = true;
            this.timeLeft = this.duration;
            PlayClip();
            SetAlpha(this.baseAlpha);
        }

        /// <summary>
        /// Awake function
        /// </summary>
        protected override void OnAwake() => this.material = GetComponent<Renderer>().material;
        #endregion

        #region Functions
        private void Start()
        {
            //Turn shield off at start
            this.baseAlpha = this.material.color.a;
            SetAlpha(0f);
        }

        private void Update()
        {
            //When shield is active
            if (!GameLogic.IsPaused && this.Active)
            {
                this.timeLeft -= Time.deltaTime;
                //Fade out slowly
                if (this.timeLeft > 0) { SetAlpha(Mathf.Lerp(0f, this.baseAlpha, this.timeLeft / this.duration)); }
                else
                {
                    //When done, turn shield off
                    this.Active = false;
                    SetAlpha(0f);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Only register collisions when active
            if (this.Active)
            {
                switch (other.tag)
                {
                    //Kill enemies
                    case "Enemy":
                        other.gameObject.GetComponent<ContactDestroy>()?.Explode();
                        PlayClip();
                        break;

                    //Destroy projectiles
                    case "Projectile_Enemy":
                        Destroy(other.gameObject);
                        PlayClip();
                        break;

                    case "Projectile":
                        if (GameLogic.IsHard && other.GetComponent<Bolt>().CanHurtPlayer)
                        {
                            Destroy(other.gameObject);
                            PlayClip();
                        }
                        break;
                }
            }
        }
        #endregion
    }
}