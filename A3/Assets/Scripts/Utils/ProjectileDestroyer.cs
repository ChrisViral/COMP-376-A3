using SpaceShooter.Physics;
using SpaceShooter.Players;
using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Player projectile destroyer
    /// </summary>
    [DisallowMultipleComponent]
    public class ProjectileDestroyer : AudioObject
    {
        #region Functions
        private void OnTriggerEnter(Collider other)
        {
            //Figure out the object's tag
            switch (other.tag)
            {
                //If a projectile, destroy both objects and trigger explosion particles
                case "Projectile":
                    if (other.GetComponent<Bolt>().Active) { PlayClip(); }
                    break;
                    
                //If player, kill player
                case "Player":
                    other.GetComponent<Player>().Die();
                    break;
            }
        }
        #endregion
    }
}