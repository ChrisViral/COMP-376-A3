using PlanetaryEscape.Players;
using UnityEngine;

namespace PlanetaryEscape.Utils
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class PlayerGroundCheck : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float repulsionForce;
        #endregion

        #region Functions
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.GetComponentInParent<Player>();
                if (!player.Die())
                {
                    player.Rigidbody.AddForce(Vector3.up * this.repulsionForce, ForceMode.VelocityChange);
                }
            }
        }
        #endregion
    }
}
