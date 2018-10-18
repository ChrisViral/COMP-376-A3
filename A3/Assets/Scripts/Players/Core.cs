
namespace SpaceShooter.Players
{
    /// <summary>
    /// Boss ship core
    /// </summary>
    public class Core : Vulnerability
    {
        #region Methods
        /// <summary>
        /// Core hit event
        /// </summary>
        protected override void OnHit() => this.boss.HitCore();
        #endregion
    }
}
