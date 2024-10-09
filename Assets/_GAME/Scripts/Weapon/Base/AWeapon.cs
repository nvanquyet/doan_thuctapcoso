using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{
    public abstract class AWeapon : AAttacker
    {
        protected float attackSpeed;

        private bool isAttacking = false;
        public virtual bool Attack(){
            if(isAttacking) return false;
            isAttacking = true;
            Invoke(nameof(ResetAttack), attackSpeed);
            return true;
        }

        public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            if (!CanAttack && Damage <= 0) return false;
            //Multiple Damage if is critrate
            isSuper = IsCritRate();
            forcePushBack = CurrentEquiqmentStat.Data.GetStat(TypeStat.WeaponForce).GetValue();
            target.Defend(Damage * (isSuper ? 2 : 1), isSuper, (forcePushBack, this.transform));
            return true;
        }

        protected bool IsCritRate() => UnityEngine.Random.value <= CurrentEquiqmentStat.Data.GetStat(TypeStat.CritRate).GetValue();


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
        [SerializeField] protected int idWeapon;
        protected EquiqmentStat equiqment;
        protected EquiqmentStat currentEquiqmentStat;
        public EquiqmentStat EquiqmentStat
        {
            get
            {
                if (equiqment.Data == null) equiqment = GameData.Instance.WeaponData.GetValue(idWeapon).ItemAttributes;
                return equiqment;
            }
        }

        public EquiqmentStat CurrentEquiqmentStat
        {
            get {
                if(currentEquiqmentStat.Data == null) currentEquiqmentStat = EquiqmentStat.Clone();
                return currentEquiqmentStat;
            }
        }
        internal void ApplyStat(IStatProvider stat)
        {
            //Aply stat to equiqment
            var data = stat.Data;
            if (data == null) return;
            //foreach (var statData in data.Stats)
            //{
            //    CurrentEquiqmentStat.Data.UpdateStat(GameService.CaculateStat(CurrentEquiqmentStat.Data.GetStat(statData.TypeStat), statData, EquiqmentStat.Data.GetStat(statData.TypeStat)));
            //}
            foreach (var statData in CurrentEquiqmentStat.Data.Stats)
            {
                CurrentEquiqmentStat.Data.UpdateStat(GameService.CaculateStat(statData, data.GetStat(statData.TypeStat), EquiqmentStat.Data.GetStat(statData.TypeStat)));
            }

            OnAttackSpeedChange();
        }

        protected virtual void OnAttackSpeedChange()
        {
            var rate = currentEquiqmentStat.Data.GetStat(TypeStat.AttackRate).Value;
            attackSpeed = 1 / (rate == 0 ? 1 : rate);
        }

        #endregion
    }

}