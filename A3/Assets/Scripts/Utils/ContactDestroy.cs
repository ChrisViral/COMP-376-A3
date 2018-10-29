using PlanetaryEscape.Physics;
using PlanetaryEscape.Players;
using PlanetaryEscape.Waves;
using UnityEngine;

namespace PlanetaryEscape.Utils
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
                    }
                    break;
                    
                //If player, kill player
                case "Player":
                    if (!other.transform.parent.GetComponent<Player>().Die()) { Explode(); }
                    break;
            }
        }
        #endregion
    }
}
