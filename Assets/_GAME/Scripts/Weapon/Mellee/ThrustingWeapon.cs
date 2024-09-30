namespace ShootingGame
{
    public class ThrustingWeapon : MelleeWeapon
    {
        public override bool Attack()
        {
            return base.Attack();
        }

        protected override void OnAttackSpeedChange()
        {
            throw new System.NotImplementedException();
        }
    }
}
