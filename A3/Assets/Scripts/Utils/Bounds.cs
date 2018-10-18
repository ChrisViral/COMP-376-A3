using System;
using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// 2D rectangular boundaries limiter
    /// </summary>
    [Serializable]
    public struct Bounds
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("X axis bounds")]
        private float xMin;
        [SerializeField]
        private float xMax;
        [SerializeField, Header("Z axis bounds")]
        private float zMin;
        [SerializeField]
        private float zMax;
        #endregion

        #region Methods
        /// <summary>
        /// Clamps the given Vector3 inside this bounds object
        /// </summary>
        /// <param name="v">Vector3 to clamp</param>
        /// <returns>Clamped vector</returns>
        public Vector3 BoundVector(Vector3 v) => new Vector3(Mathf.Clamp(v.x, this.xMin, this.xMax), 0f, Mathf.Clamp(v.z, this.zMin, this.zMax));
        #endregion
    }
}
