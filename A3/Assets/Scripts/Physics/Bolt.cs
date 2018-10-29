using System.Collections;
using UnityEngine;

namespace PlanetaryEscape.Physics
{
    /// <summary>
    /// Bolt
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class Bolt : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private GameObject toDestroy;
        #endregion

        #region Properties  
        /// <summary>
        /// State of the bolt, has not been destroyed if true
        /// Fetching this value will destroy the bolt and set this to false
        /// </summary>
        private bool active = true;
        public bool Active
        {
            get
            {
                if (this.active)
                {
                    this.active = false;
                    Destroy(this.toDestroy ? this.toDestroy : this.gameObject);
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
}
