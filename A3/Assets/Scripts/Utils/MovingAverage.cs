using System;
using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// A moving average, who's value is the average of a set number of previous values
    /// </summary>
    [Serializable]
    public class MovingAverage
    {
        #region Fields
        //Private fields
        [SerializeField, HideInInspector]
        private readonly float[] storage;
        [SerializeField, HideInInspector]
        private float total, average;
        [SerializeField, HideInInspector]
        private int index;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current average value for this MovingAverage
        /// </summary>
        public float Value
        {
            get { return this.average; }
            set
            {
                this.total += value - this.storage[this.index];
                this.storage[this.index++] = value;
                this.index %= this.storage.Length;
                this.average = this.total / this.storage.Length;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new MovingAverage set to 0
        /// </summary>
        /// <param name="size">Size of the previous values sample</param>
        public MovingAverage(int size = 10)
        {
            this.storage = new float[size];
        }

        /// <summary>
        /// Creates a new MovingAverage set to the given value
        /// </summary>
        /// <param name="size">Size of the previous values sample</param>
        /// <param name="initialValue">Starting value</param>
        public MovingAverage(int size, float initialValue) : this(size)
        {
            //Set all values to the initial
            for (int i = 0; i < size; i++)
            {
                this.storage[i] = initialValue;
                this.total += initialValue;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Debug string of this MovingAverage
        /// </summary>
        /// <returns>Debug string</returns>
        public override string ToString() => $"storage: [ {string.Join(", ", this.storage)} ], total: {this.total}, average: {this.average}";
        #endregion

        #region Operators
        /// <summary>
        /// Gets the average value of this MovingAverage
        /// </summary>
        /// <param name="m">MovingAverage to get the value from</param>
        public static implicit operator float(MovingAverage m) => m.average;
        #endregion
    }
}
