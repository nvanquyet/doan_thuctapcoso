using UnityEngine;
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
            if(_isAttacking) Invoke(nameof(Attack), 1f);
            return true;
        }
        
        private void Attack(){
            if(_currentTarget == null || !_isAttacking) return;
            Attack(_currentTarget);
        }

        public override void ExitInteract(Interface.Interact target) => _isAttacking = false;
        public override void Interact(Interface.Interact target) { }
    }
}

