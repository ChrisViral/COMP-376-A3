using SpaceShooter.Utils;
using UnityEngine;

namespace SpaceShooter.Physics
{
    /// <summary>
    /// Physical object abstract class
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PhysicsObject : PausableObject
    {
        #region Fields
        /// <summary>
        /// This object's Rigidbody component
        /// </summary>
        protected new Rigidbody rigidbody;
        #endregion

        #region Virtual methods
        /// <summary>
        /// This is called from within Awake, you should override this instead of writing an Awake() method
        /// </summary>
        protected virtual void OnAwake() { }
        #endregion

        #region Functions
        private void Awake()
        {
            //Get Rigidbody from components
            this.rigidbody = GetComponent<Rigidbody>();

            //Call children Awake method
            OnAwake();
        }
        #endregion
    }
}
