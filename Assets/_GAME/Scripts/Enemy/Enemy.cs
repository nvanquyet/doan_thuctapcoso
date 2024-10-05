using System;
using UnityEngine;

namespace ShootingGame
{
    public class Enemy : AInteractable<BoxCollider2D>
    {
        [SerializeField] private EnemyAttacker _enemyAttacker;
        [SerializeField] private EnemyDefender _enemyDefender;
        [SerializeField] private EnemyMovement _enemyMovement;
        public bool IsDead => _enemyDefender.IsDead;

        public Action OnDeadAction;
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _enemyAttacker = GetComponentInChildren<EnemyAttacker>();
            _enemyDefender = GetComponentInChildren<EnemyDefender>();
            _enemyMovement = GetComponentInChildren<EnemyMovement>();
        }
#endif

        private void Start(){
            if(_enemyMovement != null && _enemyDefender != null) {
                _enemyDefender.OnDefend += () => _enemyMovement.PauseMovement(true);
                _enemyDefender.OnDefendSuccess += () => _enemyMovement.PauseMovement(false);
                _enemyDefender.OnDeath += () =>
                {
                    OnDeadAction?.Invoke();
                    LevelSpawner.Instance.OnEnemyDeath(this);
                    Destroy(gameObject);
                };
            }
        }

        public void Init(float scaleFactor){
            _enemyDefender.Init(scaleFactor);
            _enemyAttacker.Init(scaleFactor);
            //_enemyMovement.Init(scaleFactor);
        }


        public override void OnInteract(Interface.IInteract target) {
            if(target is FireRangePlayer) target.OnInteract(this);
        } 
        
        public override void ExitInteract(Interface.IInteract target) {
            if(target is FireRangePlayer) target.OnInteract(this);
        }
    }
}