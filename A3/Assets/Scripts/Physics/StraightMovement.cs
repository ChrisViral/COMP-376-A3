using UnityEngine;

namespace PlanetaryEscape.Physics
{
    /// <summary>
    /// Gives an object a constant forward motion upon load
    /// </summary>
    [DisallowMultipleComponent, AddComponentMenu("Physics/Straight Movement")]
    public class StraightMovement : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float speed;
        #endregion

        #region Functions
        //Set requested speed
        private void Start() => this.rigidbody.velocity = Vector3.forward * this.speed;
        #endregion
    }
}
