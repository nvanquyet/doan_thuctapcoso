
using UnityEngine;
namespace ShootingGame
{

    public class FireRangePlayer : AInteractable<BoxCollider2D>
    {
        [SerializeField] private WeaponCtrl _weaponCtrl;

        private void OnValidate() {
            _weaponCtrl = transform.parent?.GetComponentInChildren<WeaponCtrl>();
        }

        public override void ExitInteract(Interface.Interact target) { 
            if(target == null) return;
            Debug.Log("Enemies Outry");
            if (target is Enemy) _weaponCtrl.RemoveEnemyToFireRange((target as Enemy).transform);
        }

        public override void Interact(Interface.Interact target) { 
            if(target == null) return;
            if(target is Enemy) {
                Debug.Log("Enemies Entry");
                var enemy = target as Enemy;
                if(enemy.IsDead) return;
                (target as Enemy).OnDeadAction += (value) =>_weaponCtrl.RemoveEnemyToFireRange(value);
                _weaponCtrl.AddEnemyToFireRange((target as Enemy).transform);
            }
        }
    }

}