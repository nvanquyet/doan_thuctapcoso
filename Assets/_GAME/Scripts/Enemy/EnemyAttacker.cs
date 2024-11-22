using System;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{
    public class EnemyAttacker : AAttacker
    {
        private Interface.IDefender _currentTarget;
        private bool _isAttacking = false;
        [SerializeField] private float timeAttack = 1f;
        private Action OnAttackAction;
        private Action OnAttackCompletedAction;

        [SerializeField] private BaseBullet projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private TypeAttack attackType;

        private IDefender denfender;

        private ADefender _target;
        public float AttackRange => attackRange;

        private void Start()
        {
            denfender = GetComponentInParent<IDefender>();
        }

        public void SetTarget(ADefender target) => _target = target;
        public void SetAttackAction(Action attackAction, Action onAttackCompletedAction)
        {
            OnAttackAction = attackAction;
            OnAttackCompletedAction = onAttackCompletedAction;
        }

        public void Update()
        {
            if (_target == null) return;
            Vector2 direction = (_target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);
            if(distanceToTarget <= attackRange)
            {
                if (!_isAttacking)
                {
                    if (attackType == TypeAttack.Melee) _isAttacking = Attack(_target);
                    else if (attackType == TypeAttack.Ranged) _isAttacking = SpawnProjectile(direction);
                    if (_isAttacking)
                    {
                        OnAttackAction?.Invoke();
                        Invoke(nameof(OnAttackCompleted), timeAttack);
                    }
                }
            }
        }

        private bool SpawnProjectile(Vector2 direction)
        {
            if (attackType == TypeAttack.Ranged && projectilePrefab != null && _target != null)
            {
                var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                projectile.Spawn(direction, (Damage, false, 0));
                return true;
            }
            return false;
        }

        public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            if (target == null || target is EnemyDefender) return false;
            _currentTarget = target;
            return true;
        }

        private void OnAttackCompleted()
        {
            OnAttackCompletedAction?.Invoke();
            _isAttacking = false;
        }

        public void Init(float scaleFactor)
        {
            SetDamage((int)(scaleFactor * Damage));
        }

        public override void ExitInteract(Interface.IInteract target) { }
        public override void OnInteract(Interface.IInteract target) { }
        protected override void OnTriggerEnter2D(Collider2D other) { }

        protected override void OnTriggerExit2D(Collider2D other) { }
        
    }
}
