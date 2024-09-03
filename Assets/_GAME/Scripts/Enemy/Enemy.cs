using UnityEngine;

namespace ShootingGame
{
    public class Enemy : AInteractor
    {
        [SerializeField] private EnemyAttacker _enemyAttacker;
        [SerializeField] private EnemyDefender _enemyDefender;
        [SerializeField] private EnemyMovement _enemyMovement;

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
        public override void Interact(Interface.Interact target)
        {
            if(target is FireRangePlayer){
                if(_enemyDefender.IsDead) return;
                WeaponCtrl.Instance.AddEnemyToFireRange(this.transform);
            }
        }

        public override void ExitInteract(Interface.Interact target)
        {
            if(target is FireRangePlayer) WeaponCtrl.Instance.RemoveEnemyToFireRange(this.transform);
        }
    }
}