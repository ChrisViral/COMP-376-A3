﻿using UnityEngine;

namespace PlanetaryEscape.Physics
{
    /// <summary>
    /// Gives a figure eight movement to an object
    /// </summary>
    [DisallowMultipleComponent, AddComponentMenu("Physics/Figure Eight Movement")]
    public class FigureEightMovement : PhysicsObject
    {
        #region Constants
        /// <summary>
        /// Period of the Cosinus function (2*Pi)
        /// </summary>
        private const float cosPeriod = 6.283185f;
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField]
        private float maxSpeed, period;

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
        
        protected override void OnFixedUpdate()
        {
            /* So, explanation time. The parametric position equations for a figure eight movement are
             * x = sin(t)
             * y = sin(t)cos(t)
             * Taking the derivative gives us the velocity equations
             * x = cos(t)
             * y = cos(2t)
             * Which we can then apply to our rigidbody */
            float t = (this.ElapsedTime / this.period) % cosPeriod;
            this.Rigidbody.velocity = new Vector3(Mathf.Cos(t), Mathf.Cos(2 * t)) * this.maxSpeed;
        }
        #endregion
    }
}
