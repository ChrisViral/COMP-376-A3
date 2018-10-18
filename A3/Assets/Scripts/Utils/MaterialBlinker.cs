using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Blinks an object by changing the material
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class MaterialBlinker : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private Material[] blinkMaterials;
        [SerializeField]
        private float blinkTime;

        //Private fields
        private Renderer renderer;
        private bool blinking;
        #endregion

        #region Methods
        /// <summary>
        /// Blinks the object
        /// </summary>
        /// <param name="index">Index of the Material to blink</param>
        public void Blink(int index)
        {
            //Start blinking if ont already blinking
            if (!this.blinking)
            {
                this.blinking = true;
                StartCoroutine(BlinkMaterial(this.blinkMaterials[index]));
            }
        }

        /// <summary>
        /// Blinks the material for a given time
        /// </summary>
        /// <param name="mat">Material to blink</param>
        private IEnumerator<YieldInstruction> BlinkMaterial(Material mat)
        {
            //Switch material
            Material original = this.renderer.material;
            this.renderer.material = mat;
            yield return new WaitForSeconds(this.blinkTime);

            //Put old material back
            this.renderer.material = original;
            this.blinking = false;
        }
        #endregion

        #region Functions
        //Get the renderer
        private void Awake() => this.renderer = GetComponent<Renderer>();
        #endregion
    }
}