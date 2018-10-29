using PlanetaryEscape.Players;
using UnityEngine;

namespace PlanetaryEscape.Utils
{
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class ShipDestroyer : MonoBehaviour
    {
        #region Functions
        //Make any ship entering the collider explode
        private void OnTriggerEnter(Collider other) => (other.GetComponent<Ship>() ?? other.GetComponentInParent<Ship>())?.Explode();
        #endregion
    }
}
