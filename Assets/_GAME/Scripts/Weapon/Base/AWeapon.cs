using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{

    public abstract class AWeapon : AAttacker
    {
        [SerializeField] protected float attackSpeed = 0.5f;
        private bool isAttacking = false;
        public virtual bool Attack(){
            if(isAttacking) return false;
            isAttacking = true;
            Invoke(nameof(ResetAttack), attackSpeed);
            return true;
        }
        private void ResetAttack() => isAttacking = false;

        public virtual void Rotate(Vector3 pos)
        {
            Vector2 lookDir = pos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;

            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) transform.localScale = new Vector3(1, -1, 0);
            else transform.localScale = new Vector3(1, 1, 0);
        }


        #region Stat
        [SerializeField] protected EquiqmentStat equiqment;

        internal void ApplyStat(IStatProvider stat)
        {
            //Aply stat to equiqment
            OnAttackSpeedChange();
        }

        protected abstract void OnAttackSpeedChange();

        #endregion
    }

}