using DG.Tweening;
using UnityEngine;

namespace ShootingGame
{
    public abstract class MelleeWeapon : AWeapon
    {

        [SerializeField] private Transform weaponTransform; 
        [SerializeField] private Transform weaponTsRotate;

        protected Transform WeaponTs
        {
            get
            {
                if(weaponTransform != null) return weaponTransform;
                return this.transform;
            }
        }

        protected Transform WeaponTsRotate
        {
            get
            {
                if(weaponTsRotate != null) return weaponTsRotate;
                return this.transform.parent;
            }
        }


        public override bool Attack()
        {
            if (base.Attack())
            {
                CreateTweenSequence();
                return true;
            }
            return false;
        }

        protected abstract void CreateTweenSequence();

        public override void Rotate(Vector3 pos)
        {
            var direction = (pos - WeaponTsRotate.parent.position).normalized;
            var time = UnityEngine.Random.Range(0.24f, 0.56f);
            WeaponTsRotate.DOMove(WeaponTsRotate.parent.position + direction, time).OnUpdate(() =>
            {
                var dir = (pos - WeaponTsRotate.parent.position).normalized;
                WeaponTsRotate.right = new Vector2(dir.x, dir.y);
            });
            
        }

        public override void GainExp(int exp)
        {
            GameService.LogColor("Gain Exp: " + exp);
            expReceiver.GainExp(exp);
        }
    }
}