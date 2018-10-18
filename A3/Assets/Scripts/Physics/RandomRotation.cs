using UnityEngine;

namespace SpaceShooter.Physics
{
    /// <summary>
    /// Gives an object a random rotation upon load
    /// </summary>
    [DisallowMultipleComponent, AddComponentMenu("Physics/Random Rotation")]
    public class RandomRotation : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float maxRotation;
        #endregion

        #region Functions
        //Give the Rigidbody a random angular velocity
        private void Start() => this.rigidbody.angularVelocity = Random.insideUnitSphere * this.maxRotation;
        #endregion
    }
}
