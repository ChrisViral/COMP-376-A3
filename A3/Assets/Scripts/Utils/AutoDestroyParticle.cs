using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Particle auto destroyer
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(ParticleSystem)), AddComponentMenu("Effects/Auto Particle Destroyer")]
    public class AutoDestroyParticle : MonoBehaviour
    {
        #region Functions
        //Set object to be destroyed as soon as the ParticleSystem has played one full cycle
        private void Start() => Destroy(this.gameObject, GetComponent<ParticleSystem>().main.duration);
        #endregion
    }
}
