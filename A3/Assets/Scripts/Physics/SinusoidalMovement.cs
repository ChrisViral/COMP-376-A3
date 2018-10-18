using UnityEngine;

namespace SpaceShooter.Physics
{
    /// <summary>
    /// Adds a sinusoidal left to right movement to a Rigidbody
    /// </summary>
    [DisallowMultipleComponent, AddComponentMenu("Physics/Sinusoidal Movement")]
    public class SinusoidalMovement : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float peakAcceleration = 6.25f, period = 1f;

        //Private fields
        private float spawnTime;
        #endregion

        #region Properties
        /// <summary>
        /// Elapsed time since this object's creation
        /// </summary>
        private float ElapsedTime => Time.fixedTime - this.spawnTime;
        #endregion

        #region Functions
        //Get spawn time
        private void Start() => this.spawnTime = Time.fixedTime;

        //Add an acceleration to the object over time
        protected override void OnFixedUpdate() => this.rigidbody.AddForce(Vector3.right * Mathf.Cos(this.ElapsedTime / this.period) * this.peakAcceleration, ForceMode.Acceleration);
        #endregion
    }
}
