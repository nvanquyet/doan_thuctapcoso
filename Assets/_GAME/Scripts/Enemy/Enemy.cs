using UnityEngine;

namespace ShootingGame
{
    public class Enemy : AInteractor
    {
        [SerializeField] private EnemyAttacker _enemyAttacker;
        [SerializeField] private EnemyDefender _enemyDefender;
        [SerializeField] private EnemyMovement _enemyMovement;
        public bool IsDead => _enemyDefender.IsDead;

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
            }
        }
        public override void Interact(Interface.Interact target) => target.Interact(this);
        
        public override void ExitInteract(Interface.Interact target) => target.ExitInteract(this);
    }
}