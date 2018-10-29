using UnityEngine;
using static UnityEngine.Mathf;

namespace PlanetaryEscape.Extensions
{
    /// <summary>
    /// Vector extension methods
    /// </summary>
    public static class VectorExtensions
    {
        #region Extension methods
        /// <summary>
        /// Clamps the x and y components of the Vector2 between the negative and positive <paramref name="value"/>
        /// </summary>
        /// <param name="v">Vector to clamp</param>
        /// <param name="value">Value to clamp between</param>
        public static void ClampTo(this Vector2 v, float value) => v.Set(Clamp(v.x, -value, value), Clamp(v.y, -value, value));

        /// <summary>
        /// Clamps the x, y, and z components of the Vector3 between the negative and positive <paramref name="value"/>
        /// </summary>
        /// <param name="v">Vector to clamp</param>
        /// <param name="value">Value to clamp between</param>
        public static void ClampTo(this Vector3 v, float value) => v.Set(Clamp(v.x, -value, value), Clamp(v.y, -value, value), Clamp(v.z, -value, value));
        #endregion
    }
}
