using UnityEngine;

namespace ShootingGame
{
    public class BossAttacker : AAttacker
    {
        private bool _isAttacking = false;
        private Interface.IDefender _currentTarget;
        public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            if (target == null || target is EnemyDefender) return false;
            base.Attack(target, isSuper);
            _currentTarget = target;
            return true;
        }

        private void Attack()
        {
            if (_currentTarget == null || !_isAttacking) return;
            Attack(_currentTarget);
        }

        public override void ExitInteract(Interface.IInteract target) { }
        public override void OnInteract(Interface.IInteract target) { }

        internal void Init(float scaleFactor)
        {
            SetDamage((int)(scaleFactor * Damage));
        }
    }
}

