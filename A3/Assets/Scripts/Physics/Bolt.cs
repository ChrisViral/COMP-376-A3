using System.Collections;
using UnityEngine;

namespace SpaceShooter.Physics
{
    /// <summary>
    /// Bolt
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class Bolt : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private int maxTeleports;
        [SerializeField]
        private float teleportDistance, sideDistance;
        [SerializeField]
        private string teleporterTag;
        [SerializeField, Tooltip("Time before this bolt can hurt the player")]
        private float playerImmunityTime;
        #endregion

        #region Properties  
        /// <summary>
        /// State of the bolt, has not been destroyed if true
        /// Fetching this value will destroy the bolt and set this to false
        /// </summary>
        private bool active = true;
        public bool Active
        {
            get
            {
                if (this.active)
                {
                    this.active = false;
                    Destroy(this.gameObject);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Remaining teleport attempts
        /// </summary>
        public int TeleportsRemaining { get; private set; }

        /// <summary>
        /// If this bolt can currently hurt the player
        /// </summary>
        public bool CanHurtPlayer { get; private set; }
        #endregion

        #region Functions
        //Set the current amount of available teleports
        protected override void OnAwake() => this.TeleportsRemaining = this.maxTeleports;

        private IEnumerator Start()
        {
            //Wait the given time before setting the hurt value to true
            yield return new WaitForSeconds(this.playerImmunityTime);
            this.CanHurtPlayer = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!GameLogic.IsHard) { return; }

            //If colliding with a teleporter, try to teleport back
            if (other.CompareTag(this.teleporterTag))
            {
                if (--this.TeleportsRemaining >= 0)
                {
                    //Move back the given distance
                    this.rigidbody.MovePosition(this.rigidbody.position + (Vector3.back * this.teleportDistance));
                }
            }
            else
            {
                switch (other.tag)
                {
                    case "Teleporter_Left":
                        if (--this.TeleportsRemaining >= 0)
                        {
                            //Move right the given distance
                            this.rigidbody.MovePosition(this.rigidbody.position + (Vector3.right * this.sideDistance));
                        }
                        break;

                    case "Teleporter_Right":
                        if (--this.TeleportsRemaining >= 0)
                        {
                            //Move left the given distance
                            this.rigidbody.MovePosition(this.rigidbody.position + (Vector3.left * this.sideDistance));
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
