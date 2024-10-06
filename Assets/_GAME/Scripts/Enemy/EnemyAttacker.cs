
namespace ShootingGame
{
    public class EnemyAttacker : AAttacker
    {
        private bool _isAttacking = false;
        private Interface.IDefender _currentTarget;
        public override bool Attack(Interface.IDefender target)
        {
            if(target == null || target is EnemyDefender) return false;
            _isAttacking = base.Attack(target);
            _currentTarget = target;
            if (_isAttacking) Invoke(nameof(Attack), UnityEngine.Random.Range(0.25f, 0.5f));
            return true;
        }
        
        private void Attack(){
            if (_currentTarget == null || !_isAttacking) return;
            Attack(_currentTarget);
        }

        public override void ExitInteract(Interface.IInteract target) => _isAttacking = false;
        public override void OnInteract(Interface.IInteract target) { }

        internal void Init(float scaleFactor)
        {
            SetDamage((int) (scaleFactor * Damage));
        }
    }
}

