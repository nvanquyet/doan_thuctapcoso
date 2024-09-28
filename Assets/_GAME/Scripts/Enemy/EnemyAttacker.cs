using System;
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
            _currentTarget = target;
            if (_isAttacking) Invoke(nameof(Attack), UnityEngine.Random.Range(0.25f, 0.5f));
            return true;
        }
        
        private void Attack(){
           Debug.Log($"attack repeat {_isAttacking} currrentTarget {_currentTarget}");
            if (_currentTarget == null || !_isAttacking) return;
            Attack(_currentTarget);
        }

        public override void ExitInteract(Interface.Interact target) => _isAttacking = false;
        public override void OnInteract(Interface.Interact target) { }

        internal void Init(float scaleFactor)
        {
            SetDamage((int) (scaleFactor * Damage));
        }
    }
}

