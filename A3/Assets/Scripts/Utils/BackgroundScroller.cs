using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Scrolls a background texture smoothly over time
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Renderer)), AddComponentMenu("Rendering/BackgroundScroller")]
    public class BackgroundScroller : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField, Tooltip("Scroll speed")]
        private float speed;

        //Private fields
        private Material material;
        #endregion

        #region Functions
        //Get the material from the renderer
        private void Awake() => this.material = GetComponent<Renderer>().material;

        //Scroll the texture by changing the texture offset
        private void Update() => this.material.SetTextureOffset("_MainTex", new Vector2(0f, (this.material.mainTextureOffset.y + (this.speed * Time.deltaTime)) % 1f));
        #endregion
    }
}