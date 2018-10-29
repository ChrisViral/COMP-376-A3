using System.Collections;
using UnityEngine;

namespace PlanetaryEscape.Utils
{
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class CameraShake : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float duration = 1f, intensity = 1f, smoothing = 5f;

        //Private fields
        private bool shaking;
        #endregion

        #region Methods
        /// <summary>
        /// Starts the camera shaking sequence
        /// </summary>
        public void Shake()
        {
            if (!this.shaking) { StartCoroutine(ShakeCamera());  }
        }

        /// <summary>
        /// Shakes the camera over time
        /// </summary>
        private IEnumerator ShakeCamera()
        {
            this.shaking = true;
            Quaternion original = this.transform.localRotation;
            Vector3 euler = original.eulerAngles;
            for (float remaining = this.duration; remaining >= 0f; remaining -= Time.deltaTime)
            {
                Vector3 random = Random.insideUnitSphere * this.intensity * (remaining / this.duration);
                remaining = Mathf.Lerp(remaining, 0f, Time.deltaTime);
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(euler + (Random.insideUnitSphere * this.intensity)), Time.deltaTime * this.smoothing);
                yield return null;
            }
            this.transform.localRotation = original;
            this.shaking = false;
        }
        #endregion
    }
}
