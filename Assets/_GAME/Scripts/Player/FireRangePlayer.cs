
using UnityEngine;
namespace ShootingGame
{

    public class FireRangePlayer : AInteractable<BoxCollider2D>
    {
        //        [SerializeField] private WeaponCtrl _weaponCtrl;
        //#if UNITY_EDITOR
        //        protected override void OnValidate()
        //        {
        //            base.OnValidate();
        //            _weaponCtrl = transform.parent?.GetComponentInChildren<WeaponCtrl>();
        //        }
        //#endif

        //        public override void ExitInteract(Interface.Interact target) { 
        //            if(target == null) return;
        //            Debug.Log("Enemies Outry");
        //            if (target is Enemy) _weaponCtrl.RemoveEnemyToFireRange((target as Enemy).transform);
        //        }

        //        public override void Interact(Interface.Interact target) { 
        //            Debug.Log($"{target} type {target.GetType()}");
        //            if(target == null) return;
        //            if(target is Enemy) {
        //                Debug.Log("Object");
        //                var enemy = target as Enemy;
        //                if(enemy.IsDead) return;
        //                enemy.OnDeadAction += () =>
        //                {
        //                    _weaponCtrl.RemoveEnemyToFireRange(enemy.transform);
        //                    LevelSpawner.Instance.OnEnemyDeath(enemy);
        //                };
        //                _weaponCtrl.AddEnemyToFireRange(enemy.transform);
        //            }
        //        }
        public override void ExitInteract(Interface.Interact target)
        {
           
        }

        public override void OnInteract(Interface.Interact target)
        {
            //throw new System.NotImplementedException();
            //throw new System.NotImplementedException();
        }
    }

}