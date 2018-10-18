using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Game boundaries limiter
    /// </summary>
    [RequireComponent(typeof(Collider)), AddComponentMenu("Physics/Boundary")]
    public class GameBoundary : MonoBehaviour
    {
        #region Functions
        //If the object exits the boundary, destroy it
        private void OnTriggerExit(Collider other) => Destroy(other.gameObject);
        #endregion
    }
}
