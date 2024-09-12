using System;
using UnityEngine;

namespace ShootingGame
{
    public class Enemy : AInteractor
    {
        [SerializeField] private EnemyAttacker _enemyAttacker;
        [SerializeField] private EnemyDefender _enemyDefender;
        [SerializeField] private EnemyMovement _enemyMovement;
        public bool IsDead => _enemyDefender.IsDead;

        public Action<Transform> OnDeadAction;

        private void OnValidate()
        {
            _enemyAttacker = GetComponentInChildren<EnemyAttacker>();
            _enemyDefender = GetComponentInChildren<EnemyDefender>();
            _enemyMovement = GetComponentInChildren<EnemyMovement>();
        }

        private void Start(){
            if(_enemyMovement != null && _enemyDefender != null) {
                _enemyDefender.OnDefend += () => _enemyMovement.PauseMovement(true);
                _enemyDefender.OnDefendSuccess += () => _enemyMovement.PauseMovement(false);
                _enemyDefender.OnDeath += () =>
                {
                    OnDeadAction?.Invoke(this.transform);
                    Destroy(gameObject);
                };
            }
        }

        public void Init(float scaleFactor){
            _enemyDefender.Init(scaleFactor);
            _enemyAttacker.Init(scaleFactor);
            //_enemyMovement.Init(scaleFactor);
        }


        public override void Interact(Interface.Interact target) {
            if(target is FireRangePlayer) target.Interact(this);
        } 
        
        public override void ExitInteract(Interface.Interact target) {
            if(target is FireRangePlayer) target.Interact(this);
        }
    }
}