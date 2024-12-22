using JetBrains.Annotations;
using System;
using UnityEngine;

namespace ShootingGame
{
    [Serializable]
    public struct EnemyWeights
    {
        public float HpWeight;
        public float DamageWeight;
        public float SpeedWeight;

        public EnemyWeights(float hpWeight, float damageWeight, float speedWeight)
        {
            HpWeight = hpWeight;
            DamageWeight = damageWeight;
            SpeedWeight = speedWeight;
        }
    }
    public class Enemy : AInteractable<BoxCollider2D>
    {
        [SerializeField] private EnemyAttacker _enemyAttacker;
        [SerializeField] private EnemyDefender _enemyDefender;
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyAnimation _enemyAnimation;
        public bool IsDead => _enemyDefender.IsDead;

        public Action<Interface.IAttacker> OnDeadAction;
        public Action<float> OnDefendAction;
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _enemyAttacker = GetComponentInChildren<EnemyAttacker>();
            _enemyDefender = GetComponentInChildren<EnemyDefender>();
            _enemyMovement = GetComponentInChildren<EnemyMovement>();
            _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        }
#endif

        private void Start()
        {
            if (_enemyMovement != null && _enemyDefender != null)
            {
                OnDeadAction += (attacker) =>
                {
                    if (attacker != null) LevelSpawner.Instance.OnEnemyDeath(this);
                    _enemyMovement.PauseMovement(true);
                    _enemyAnimation.OnTriggerDead();
                    Destroy(gameObject, 0.32f);
                };
                _enemyMovement.OnRandomTarget = GetTarget;
                _enemyMovement.OnMoveAction += _enemyAnimation.SetVelocity;
                _enemyDefender.OnDefend += (healthPercent) =>
                {
                    _enemyMovement.PauseMovement(true);
                    _enemyAnimation.OnTriggerDefend();
                    OnDefendAction?.Invoke(healthPercent);
                };
                _enemyDefender.OnDefendSuccess += () => _enemyMovement.PauseMovement(false);
                _enemyAttacker.SetAttackAction(() =>
                {
                    _enemyMovement.PauseMovement(true);
                    _enemyMovement.SetAttackRange(_enemyAttacker.DistanceAttack);
                }, () =>
                {
                    _enemyMovement.PauseMovement(false);
                });
                _enemyMovement.SetAttackRange(_enemyAttacker.DistanceAttack);
                _enemyDefender.OnDeath += OnDeadAction;
            }
        }

        public void Init(EnemyPropData data, int currentWave, bool isBoss, Action<Sprite> OnCallback = null)
        {
            GetTarget();
            var growthRate = Mathf.Pow(data.Properties.GrowthRate, currentWave - 1);
            _enemyDefender.Init(data.Properties.BaseHealth, data.Properties.BaseEXP, data.Properties.BaseCoin, growthRate);
           
            _enemyAttacker.Init(data.Properties.BaseDamage, growthRate);
            _enemyMovement.Init(data.Properties.BaseSpeed, growthRate);
            if (data.Icon) OnCallback?.Invoke(data.Icon);
            if (Collider is BoxCollider2D boxCollider2D)
            {
                boxCollider2D.size = _enemyAnimation.SpriteRenderer.bounds.size;
                Vector2 spriteCenter = new Vector2(_enemyAnimation.SpriteRenderer.bounds.center.x,
                                                    _enemyAnimation.SpriteRenderer.bounds.center.y);
                boxCollider2D.offset = spriteCenter - (Vector2)transform.position;
            }
        }

        public int GetStrength()
        {
            //Caculate strength of enemy
            return (int)(_enemyDefender.MaxHealth * GameConfig.Instance.enemyWeights.HpWeight +
                            _enemyAttacker.Damage * GameConfig.Instance.enemyWeights.DamageWeight +
                            _enemyMovement.Speed * GameConfig.Instance.enemyWeights.SpeedWeight);
        }


        public override void OnInteract(Interface.IInteract target) { }


        public override void ExitInteract(Interface.IInteract target) { }

        public virtual void GetTarget()
        {
            var _target = GameCtrl.Instance.GetRandomPlayer().Defender;
            _enemyAttacker.SetTarget(_target);
            _enemyMovement.SetTarget(_target.transform);
        }

        public void ForceDead()
        {
            _enemyDefender.Defend(_enemyDefender.MaxHealth);
            if (IsDead) _enemyDefender.OnDead(null);
        }
    }
}