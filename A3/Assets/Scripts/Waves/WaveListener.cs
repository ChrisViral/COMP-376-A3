using PlanetaryEscape.Players;
using PlanetaryEscape.Utils;
using UnityEngine;

namespace PlanetaryEscape.Waves
{
    [RequireComponent(typeof(EnemyWaveController))]
    public class WaveListener : MonoBehaviour
    {
        #region Fields
        //Private fields
        private int killed, destroyed;
        #endregion

        #region Properties
        /// <summary>
        /// Amount of objects this listener watches
        /// </summary>
        public int Count { get; internal set; }
        #endregion

        #region Methods
        /// <summary>
        /// Call this to indicate a part of the wave has been killed
        /// </summary>
        public void OnKilled() => this.killed++;

        /// <summary>
        /// Call this to indicate a part of the wave has been destroyed
        /// </summary>
        public void OnDestroyed()
        {
            if (++this.destroyed == this.Count)
            {
                //If they have also all been killed, send a completion message
                if (this.killed == this.Count)
                {
                    GameLogic.CurrentGame.WaveDestroyed();
                }

                //Destroy only this script
                Destroy(this.gameObject);
            }
        }
        #endregion
    }
}
