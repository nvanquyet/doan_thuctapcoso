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
        [SerializeField] private int id;
        [SerializeField] private EnemyAttacker _enemyAttacker;
        [SerializeField] private EnemyDefender _enemyDefender;
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyAnimation _enemyAnimation;
        [SerializeField] private bool isBoss;
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

            //split to get number of last string
            var split = gameObject.name.Split(' ');
            if (split.Length > 1)
            {
                id = int.Parse(split[split.Length - 1]) - 1;
            }
            isBoss = gameObject.name.Contains("Boss");
        }
#endif

        private void Start()
        {
            if (_enemyMovement != null && _enemyDefender != null)
            {
                OnDeadAction += (attacker) =>
                {
                    if(attacker != null) LevelSpawner.Instance.OnEnemyDeath(this);
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

        public void Init(int currentWave, Action<Sprite> OnCallback = null)
        {
            GetTarget();
            var enemyPropertiesData = isBoss ? GameData.Instance.BosssProperties.GetValue(id) : GameData.Instance.EnemyProperties.GetValue(id);
            var growthRate = Mathf.Pow(enemyPropertiesData.GrowthRate, currentWave - 1);
            _enemyDefender.Init(enemyPropertiesData.BaseHealth, enemyPropertiesData.BaseEXP, enemyPropertiesData.BaseCoin, growthRate);
            _enemyAttacker.Init(enemyPropertiesData.BaseDamage, growthRate);
            _enemyMovement.Init(enemyPropertiesData.BaseSpeed, growthRate);
            if (enemyPropertiesData.Icon) OnCallback?.Invoke(enemyPropertiesData.Icon);
        }

        public int GetStrength()
        {
            //Caculate strength of enemy
            return (int) (_enemyDefender.MaxHealth * GameConfig.Instance.enemyWeights.HpWeight + 
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