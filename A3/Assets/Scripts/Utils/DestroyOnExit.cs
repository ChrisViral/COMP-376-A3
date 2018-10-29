using UnityEngine;

namespace PlanetaryEscape.Utils
{
    /// <summary>
    /// Destroys the object as it leaves the camera's view
    /// </summary>
    [DisallowMultipleComponent]
    public class DestroyOnExit : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private GameObject toDestroy;
        #endregion

        #region Methods
        //Destroy the passed GO, or this one if the other is null
        private void OnBecameInvisible() => Destroy(this.toDestroy ? this.toDestroy : this.gameObject);
        #endregion
    }
}
