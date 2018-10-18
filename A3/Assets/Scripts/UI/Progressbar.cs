using SpaceShooter.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.UI
{
    /// <summary>
    /// Progressbar
    /// </summary>
    [AddComponentMenu("UI/Progressbar")]
    public class Progressbar : PausableObject
    {
        #region Constant
        /// <summary>
        /// Size of the used MovingAverage for progressbar smoothing
        /// </summary>
        private const int averageSize = 5;
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField]
        private RectTransform bar;
        [SerializeField]
        private Text label;
        [SerializeField, Range(0f, 1f)]
        private float progress = 1f;
        [SerializeField]
        private bool scaling;

        //Private fields
        private MovingAverage average;
        private bool wasScaling = true;
        private Vector2 originalSize;
        #endregion

        #region Properties
        private float p;
        /// <summary>
        /// Progressbar's completion percentage, setting this will update the UI
        /// </summary>
        public float Progress
        {
            get { return this.p; }
            set { this.p = Mathf.Clamp01(value); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the bar to the given fill value
        /// </summary>
        /// <param name="fill">Fill value of the progressbar (between 0 and 1)</param>
        private void UpdateBar(float fill)
        {
            fill = Mathf.Clamp01(fill);
            this.bar.sizeDelta = new Vector2(fill * this.originalSize.x, this.originalSize.y);
            this.label.text = $"{(int)Mathf.Round(fill * 100f)}%";
        }
        #endregion

        #region Functions
        private void Start()
        {
            //Get the needed data
            this.originalSize = this.bar.rect.size;
            this.Progress = this.progress;
            this.average = new MovingAverage(averageSize, this.Progress);
        }

        protected override void OnUpdate()
        {
            //If scaling the bar
            if (this.scaling)
            {
                this.progress = Mathf.Clamp01(this.progress);
                UpdateBar(this.progress);
                this.wasScaling = true;
            }
            //Not scaling
            else
            {
                //Reset from scaling
                if (this.wasScaling)
                {
                    this.Progress = this.progress;
                    this.average = new MovingAverage(averageSize, Mathf.Clamp01(this.Progress));
                    this.wasScaling = false;
                }
                
                //Set the bar
                this.average.Value = this.Progress;
                UpdateBar(this.average);
            }
        }
        #endregion
    }
}