using UnityEngine;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Powerup
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Powerup : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// If the Powerup is currently active or not
        /// </summary>
        public bool Active { get; private set; } = true;
        #endregion

        #region Functions
        private void OnTriggerEnter(Collider other)
        {
            //When colliding with the player, upgrade him
            if (this.Active && other.CompareTag("Player"))
            {
                other.GetComponent<Player>().IncrementLevel();
                this.Active = false;
                Destroy(this.gameObject);
            }
        }
        #endregion
    }
}
