using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Restricts the field of view of a Camera to the given aspect ratio
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera)), AddComponentMenu("Camera/Aspect Ratio Limiter")]
    public class CameraAspectRatio : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float aspectRatio;
        #endregion

        #region Functions
        private void Awake()
        {
            //Set camera aspect ratio as needed
            Camera cam = GetComponent<Camera>();
            float variance = this.aspectRatio / cam.aspect;
            if (variance < 1f) { cam.rect = new Rect((1f - variance) / 2f, 0f, variance, 1f); }
            else
            {
                variance = 1f / variance;
                cam.rect = new Rect(0, (1f - variance) / 2f, 1f, variance);
            }
        }
        #endregion
    }
}