using System.Collections.Generic;
using UnityEngine;

namespace PlanetaryEscape.Physics
{
    /// <summary>
    /// Creates a side to side movement with animations for an object
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Animator))]
    public class SideMovement : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float speed, jump, stopDrag;
        [SerializeField]
        private int passes;

        //Private fields
        private Animator animator;
        private bool isMoving = false;
        #endregion

        #region Properties
        /// <summary>
        /// Wait time before stopping the object
        /// </summary>
        public float StopWait { get; internal set; } = 4f;
        #endregion

        #region Methods
        /// <summary>
        /// Toggles between the side to side states
        /// </summary>
        public void ToggleState()
        {
            //Change from moving to stopped
            this.isMoving = !this.isMoving;
            if (this.isMoving)
            {
                //If moving set velocity
                this.Rigidbody.velocity = this.transform.right * this.speed;
                if (this.passes == 0)
                {
                    //If all passes done, prepare to stop vessel
                    this.animator.SetBool("Active", false);
                    StartCoroutine(StopVessel()); 
                }
            }
            else
            {
                //If not moving, stop vessel and teleport forward, and prepare to go the other way
                this.Rigidbody.velocity = Vector3.zero;
                this.speed *= -1f;
                this.transform.position += this.transform.forward * this.jump;
                this.passes--;
            }
        }
        
        /// <summary>
        /// Stops the vessel after a set period of time
        /// </summary>
        private IEnumerator<YieldInstruction> StopVessel()
        {
            //Wait for the animation to finish plus a given time, then stop the vessel using drag (it's simpler tbh)
            yield return new WaitForSeconds(2f + this.StopWait);
            this.Rigidbody.drag = this.stopDrag;
        }
        #endregion

        #region Functions
        protected override void OnAwake()
        {
            //Get animator
            this.animator = GetComponent<Animator>();

            //Set animation and speed correctly depending on location
            if (this.transform.position.x < 0f) { this.animator.SetTrigger("Left"); }
            else
            {
                this.animator.SetTrigger("Right");
                this.speed *= -1f;
            }
        }
        #endregion
    }
}
