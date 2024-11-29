using System;
using System.Collections.Generic;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{
    public enum TypeAttack
    {
        Melee,
        Ranged
    }
    public class EnemyAttacker : AAttacker
    {
        private Interface.IDefender _currentTarget;
        private bool _isAttacking = false;
        [SerializeField] private float timeAttack = 1f;
        [SerializeField] private float timeAttackAnimation = 1f;
        private Action OnAttackAction;
        private Action OnAttackCompletedAction;

        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private TypeAttack attackType;

        private ObjectPooling<Projectile> projectilePool;
        private IDefender defenderOwner;

        private ADefender _target;
        public float AttackRange => attackRange;
        private List<Projectile> currentProjectile = new List<Projectile>();
        private void Start()
        {
            defenderOwner = GetComponentInParent<IDefender>();
            if(attackType == TypeAttack.Ranged && projectilePrefab) projectilePool = new ObjectPooling<Projectile>(projectilePrefab, 4, transform);
            InvokeRepeating(nameof(AutoAttack), 0, timeAttack);
        }

        public void SetTarget(ADefender target) => _target = target;
        public void SetAttackAction(Action attackAction, Action onAttackCompletedAction)
        {
            OnAttackAction = attackAction;
            OnAttackCompletedAction = onAttackCompletedAction;
        }

        public void AutoAttack()
        {
            if (_target == null) return;
            if (!_isAttacking)
            {
                if (attackType == TypeAttack.Melee)
                {
                    var distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);
                    if (distanceToTarget <= attackRange) _isAttacking = Attack(_target);
                    else _isAttacking = false;
                }
                else if (attackType == TypeAttack.Ranged)
                {
                    var direction = (_target.transform.position - transform.position).normalized;
                    _isAttacking = SpawnProjectile(direction);
                }
                if (_isAttacking)
                {
                    OnAttackAction?.Invoke();
                    Invoke(nameof(OnAttackCompleted), timeAttackAnimation);
                }
            }
        }

        private bool SpawnProjectile(Vector2 direction)
        {
            if (attackType == TypeAttack.Ranged && _target != null)
            {
                var projectile = projectilePool.Get();
                projectile.transform.SetParent(null);
                projectile.transform.position = projectileSpawnPoint.position;
                projectile.Spawn(direction, new ImpactData(Damage, false, 0), defenderOwner);
                projectile.OnRecycle = () => {
                    if (projectile == null) return;
                    else
                    {
                        projectile.transform.SetParent(transform);
                        currentProjectile.Remove(projectile);
                        projectilePool.Recycle(projectile);
                    }
                };
                currentProjectile.Add(projectile);
                return true;
            }
            return false;
        }


        private void OnDestroy()
        {
            if(currentProjectile.Count > 0)
            {
                foreach (var projectile in currentProjectile)
                {
                    Destroy(projectile.gameObject);
                }
            }
        }

        public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            if (target == null || target is EnemyDefender) return false;
            _currentTarget = target;
            target.Defend(Damage, isSuper, (forcePushBack, transform));
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
