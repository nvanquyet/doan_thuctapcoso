using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{
    public abstract class AItem : AAttacker
    {
        [SerializeField] protected SpriteRenderer weaponSprite;


        private void SetSprite(Sprite sprite) => weaponSprite.sprite = sprite;

        #region Stat
        protected StatContainerData equiqment;
        protected StatContainerData currentEquiqmentStat;
        public StatContainerData EquiqmentStat => equiqment;

        public StatContainerData CurrentEquiqmentStat
        {
            get
            {
                if (currentEquiqmentStat == null) currentEquiqmentStat = new StatContainerData(EquiqmentStat);
                return currentEquiqmentStat;
            }
        }

        public void InitializeItem(ItemAttributeData data)
        {
            equiqment = data.Stat;
            SetSprite(data.Appearance.Icon);
        }

        public virtual void ApplyStat(StatContainerData stat)
        {
            //Aply stat to equiqment
            if (stat == null) return;
            foreach (var statData in CurrentEquiqmentStat.Stats)
            {
                CurrentEquiqmentStat.UpdateStat(GameService.CaculateStat(statData, stat.GetStat(statData.TypeStat), EquiqmentStat.GetStat(statData.TypeStat)));
            }
        }

        #endregion
    }
    public abstract class AWeapon : AItem
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
            forcePushBack = CurrentEquiqmentStat.GetStat(TypeStat.WeaponForce).GetValue();
            target.Defend(Damage * (isSuper ? 2 : 1), isSuper, (forcePushBack, this.transform));
            return true;
        }

        protected bool IsCritRate() => UnityEngine.Random.value <= CurrentEquiqmentStat.GetStat(TypeStat.CritRate).GetValue();


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


        public override void ApplyStat(StatContainerData stat)
        {
            base.ApplyStat(stat);

            OnAttackSpeedChange();
        }

        protected virtual void OnAttackSpeedChange()
        {
            var rate = CurrentEquiqmentStat.GetStat(TypeStat.AttackRate).Value;
            attackSpeed = 1 / (rate == 0 ? 1 : rate);
        }

    }

}