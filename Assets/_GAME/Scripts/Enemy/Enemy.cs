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
        [SerializeField] private float growthRate = 1f;
        public bool IsDead => _enemyDefender.IsDead;

        public Action<Interface.IAttacker> OnDeadAction;
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
                OnDeadAction += (_) =>
                {
                    LevelSpawner.Instance.OnEnemyDeath(this);
                    _enemyMovement.PauseMovement(true);
                    _enemyAnimation.OnTriggerDead();
                    Destroy(gameObject, 0.32f);
                };
                _enemyMovement.OnRandomTarget = GetTarget;
                _enemyMovement.OnMoveAction += _enemyAnimation.SetVelocity;
                _enemyDefender.OnDefend += () =>
                {
                    _enemyMovement.PauseMovement(true);
                    _enemyAnimation.OnTriggerDefend();
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

        public void Init(int currentWave)
        {
            _enemyDefender.Init(growthRate, currentWave);
            _enemyAttacker.Init(growthRate, currentWave);
            _enemyMovement.Init(growthRate, currentWave);
        }

        public int GetStrength()
        {
            //Caculate strength of enemy
            return (int) (GameService.ApplyWeightToValue(_enemyDefender.MaxHealth, GameConfig.Instance.enemyWeights.HpWeight) +
                GameService.ApplyWeightToValue(_enemyAttacker.Damage, GameConfig.Instance.enemyWeights.DamageWeight) +
                GameService.ApplyWeightToValue(_enemyMovement.Speed, GameConfig.Instance.enemyWeights.SpeedWeight));
        }


        public override void OnInteract(Interface.IInteract target) { }
        

        public override void ExitInteract(Interface.IInteract target) { }

        public virtual void GetTarget()
        {
            var _target = GameCtrl.Instance.GetRandomPlayer().Defender;
            _enemyAttacker.SetTarget(_target);
            _enemyMovement.SetTarget(_target.transform);
        }
    }
}