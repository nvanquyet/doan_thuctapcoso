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
            // var direction = (pos - WeaponTsRotate.parent.position).normalized;
            // WeaponTsRotate.position = WeaponTsRotate.parent.position + direction;
            // WeaponTsRotate.right = new Vector2(direction.x, direction.y);
        }
    }
}