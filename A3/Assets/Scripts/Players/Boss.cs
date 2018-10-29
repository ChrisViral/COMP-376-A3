using System.Collections;
using System.Collections.Generic;
using PlanetaryEscape.Physics;
using PlanetaryEscape.Scenes;
using PlanetaryEscape.UI;
using PlanetaryEscape.Utils;
using UnityEngine;

namespace PlanetaryEscape.Players
{
    /// <summary>
    /// Boss ship
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(FigureEightMovement))]
    public class Boss : Ship
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Vulnerabilities")]
        private GameObject vulnerability;
        [SerializeField]
        private GameObject core;
        [SerializeField]
        private float coreTime, vulnerabilityDelay;
        [SerializeField]
        private AudioClip vulnerabilitySound;
        [SerializeField]
        private float vulnerabilityVolume;
        [SerializeField, Tooltip("Vulnerabilities location")]
        private Transform[] vulnerabilities;
        [SerializeField, Header("Boss fight"), Tooltip("Guns location")]
        private Transform[] guns;
        [SerializeField]
        private int explosionCount, score;
        [SerializeField]
        private int maxHealth; 
        [SerializeField]
        private MaterialBlinker blinker;

        //Private fields
        private Player player;
        private int hp;
        private float maxHP;
        private Progressbar healthbar;
        private FigureEightMovement movement;
        #endregion
        
        #region Properties
        /// <summary>
        /// Number of active vulnerabilities on this boss ship
        /// </summary>
        public int ActiveVulnerabilities { get; private set; }

        /// <summary>
        /// If the Core of this Boss ship is visible
        /// </summary>
        public bool CoreVisible
        {
            get { return this.core.activeSelf; }
            set { this.core.SetActive(value); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the fight coroutine
        /// </summary>
        private void StartFight() => StartCoroutine(StartFightDelayed());

        /// <summary>
        /// Fight start coroutine-
        /// </summary>
        private IEnumerator<YieldInstruction> StartFightDelayed()
        {
            //Start moving
            this.movement.enabled = true;

            //Wait the delay then start shooting and add vulnerabilities
            yield return new WaitForSeconds(this.vulnerabilityDelay);
            this.canShoot = true;
            AddVulnerabilities();
        }

        /// <summary>
        /// Adds new vulnerabilities to the boss ship
        /// </summary>
        private void AddVulnerabilities()
        {
            //Set max amount of vulnerabilities
            this.ActiveVulnerabilities = this.vulnerabilities.Length;

            //Loop through all transforms and add a vulnerability
            foreach (Transform t in this.vulnerabilities)
            {
                //Instantiate the Vulnerability as a child of the anchor point
                Instantiate(this.vulnerability, t, false);
            }
        }

        /// <summary>
        /// Vulnerability hit event
        /// </summary>
        public void HitVulnerability()
        {
            this.source.PlayOneShot(this.vulnerabilitySound, this.vulnerabilityVolume);
            this.blinker.Blink(0);
        }

        /// <summary>
        /// Vulnerability destroyed event
        /// </summary>
        public void DestroyVulnerability()
        {
            //If no more vulnerabilities are left, expose the core
            if (!this.core.activeSelf && --this.ActiveVulnerabilities == 0)
            {
                ExposeCore();
            }
        }

        /// <summary>
        /// Exposes the Boss' core
        /// </summary>
        public void ExposeCore()
        {
            //Set the core active, then start the shutoff timer
            this.CoreVisible = true;
            StartCoroutine(ShutoffCore());
        }

        /// <summary>
        /// Core shutoff coroutine
        /// </summary>
        private IEnumerator<YieldInstruction> ShutoffCore()
        {
            //Wait the given delay then turn core off
            yield return new WaitForSeconds(this.coreTime);
            this.CoreVisible = false;

            //Wait the given delay, then add new vulnerabilities
            yield return new WaitForSeconds(this.vulnerabilityDelay);
            AddVulnerabilities();
        }

        /// <summary>
        /// Core hit event
        /// </summary>
        public void HitCore()
        {

            this.source.PlayOneShot(this.vulnerabilitySound, this.vulnerabilityVolume);
            this.healthbar.Progress = --this.hp / this.maxHP;

            //If out of HP, kill the Boss
            if (this.hp == 0)
            {
                Die();
            }
            else
            {
                //Else blink
                this.blinker.Blink(1);
            }
        }

        /// <summary>
        /// Die event
        /// </summary>
        /// <returns>True if the boss dies</returns>
        public override bool Die()
        {
            if (base.Die())
            {
                //Stop movement
                this.movement.enabled = false;
                GameLogic.CurrentGame.Score += this.score;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Explodes the boss ship and ends the game
        /// </summary>
        public override void Explode()
        {
            GameLogic.CurrentGame.EndGame(true);
            base.Explode();
        }

        /// <summary>
        /// Fire event
        /// </summary>
        protected override void Fire()
        {
            Transform g;
            if (GameLogic.IsHard && this.player != null)
            {
                g = this.guns[0];
                float distance = Mathf.Abs(g.position.x - this.player.transform.position.x);
                for (int i = 1; i < this.guns.Length; i++)
                {
                    Transform t = this.guns[i];
                    float d = Mathf.Abs(t.position.x - this.player.transform.position.x);
                    if (d < distance)
                    {
                        g = t;
                        distance = d;
                    }
                }
            }
            //Get random gun
            else { g = this.guns[Random.Range(0, this.guns.Length)]; }

            //Fire at a random gun location
            Instantiate(this.bolt, g.position, Quaternion.identity);
            this.source.PlayOneShot(this.boltSound, this.shotVolume);
        }
        #endregion

        #region Functions
        private void Start()
        {
            //Setup boss
            this.player = GameLogic.CurrentGame.player;
            this.healthbar = GameLogic.CurrentGame.bossProgressbar;
            this.maxHP = this.hp = this.maxHealth * (GameLogic.IsHard ? 2 : 1);
            this.movement = GetComponent<FigureEightMovement>();
        }
        #endregion
    }
}