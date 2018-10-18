using SpaceShooter.Physics;
using SpaceShooter.Players;
using SpaceShooter.Waves;
using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Contact destructor object
    /// </summary>
    [RequireComponent(typeof(Collider)), AddComponentMenu("Physics/Contact Destroyer")]
    public class ContactDestroy : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private int points;
        [SerializeField]
        private GameObject explosion;
        #endregion

        #region Properties
        /// <summary>
        /// The WaveListener this object is associated to
        /// </summary>
        public WaveListener Listener { get; internal set; }

        /// <summary>
        /// If this object has already exploded or not
        /// </summary>
        public bool IsExploded { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Explodes this object
        /// </summary>
        public void Explode()
        {
            //Destroy and spawn explosion if not already exploded
            if (!this.IsExploded)
            {
                Destroy(this.gameObject);
                if (this.explosion != null) { Instantiate(this.explosion, this.transform.position, Quaternion.identity); }
                this.IsExploded = true;
            }
        }
        #endregion

        #region Function
        private void OnTriggerEnter(Collider other)
        {
            //Do not collide if exploded
            if (this.IsExploded) { return; }
            
            //Figure out the object's tag
            switch (other.tag)
            {
                //If a projectile, destroy both objects and trigger explosion particles
                case "Projectile":
                    if (other.GetComponent<Bolt>().Active)
                    {
                        Explode();
                        GameLogic.CurrentGame.Score += this.points;
                        if (this.Listener != null) { this.Listener.OnKilled(); }
                    }
                    break;
                    
                //If player, kill player
                case "Player":
                    if (!other.GetComponent<Player>().Die()) { Explode(); }
                    break;
            }
        }

        private void OnDestroy()
        {
            //Let the listener know the object has been destroyed if any is present
            if (this.Listener != null) { this.Listener.OnDestroyed(); }
        }
        #endregion
    }
}
