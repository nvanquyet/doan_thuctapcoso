
using System;
using UnityEngine;

namespace ShootingGame
{
    public class EnemyAttacker : AAttacker
    {
        private Interface.IDefender _currentTarget;
        private bool _isAttacking = true;
        [SerializeField] private float timeAttack = 1f;
        private Action OnAttackAction;
        private Action OnAttackCompletedAction;

        private ADefender _target;

        public void SetTarget(ADefender target)
        {
            _target = target;
        }

        public void SetAttackAction(Action attackAction, Action onAttackCompletedAction)
        {
            OnAttackAction = attackAction;
            OnAttackCompletedAction = onAttackCompletedAction;
        }

        public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            if (target == null || target is EnemyDefender) return false;
            if (_currentTarget != _target as Interface.IDefender) return false; 
            _isAttacking = base.Attack(target, isSuper);
            if (_isAttacking)
            {
                _currentTarget = target;
                if(_currentTarget is MonoBehaviour) GameService.LogColor($"Enemy Attack: {(_currentTarget as MonoBehaviour).name}");
                Invoke(nameof(OnAttackCompleted), timeAttack / 2);
                Invoke(nameof(Attack), timeAttack);
                return true;
            }
            return false;
        }

        private void OnAttackCompleted()
        {
            OnAttackCompletedAction?.Invoke();
        }

        private void Attack()
        {
            if (_currentTarget == null || !_isAttacking) return;
            _isAttacking = false;
            Attack(_currentTarget);
            OnAttackAction?.Invoke();
        }

        public override void ExitInteract(Interface.IInteract target) { }
        public override void OnInteract(Interface.IInteract target) { }

        internal void Init(float scaleFactor)
        {
            SetDamage((int)(scaleFactor * Damage));
        }
    }
}

