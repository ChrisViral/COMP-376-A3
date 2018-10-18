using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Audio object playing a single clip base class
    /// </summary>
    [RequireComponent(typeof(AudioSource)), AddComponentMenu("Audio/Audio Object")]
    public class AudioObject : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        protected AudioClip clip;
        [SerializeField]
        protected float volume;

        //Private fields
        protected AudioSource source;
        #endregion

        #region Virtual methods
        /// <summary>
        /// Plays this object's clip at the given volume
        /// </summary>
        public virtual void PlayClip() => this.source.PlayOneShot(this.clip, this.volume);

        /// <summary>
        /// This is called from within Awake, you should override this instead of writing an Awake() method
        /// </summary>
        protected virtual void OnAwake() { }
        #endregion

        #region Functions
        private void Awake()
        {
            //Gets the AudioSource
            this.source = GetComponent<AudioSource>();

            //Call children Awake method
            OnAwake();
        }
        #endregion
    }
}
