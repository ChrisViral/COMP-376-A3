using UnityEngine;

namespace PlanetaryEscape.Utils
{
    /// <summary>
    /// Destroys the object as it leaves the camera's view
    /// </summary>
    [DisallowMultipleComponent]
    public class DestroyOnExit : MonoBehaviour
    {
        #region Methods
        private void OnBecameInvisible() => Destroy(this.gameObject);
        #endregion
    }
}
