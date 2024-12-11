using UnityEngine;
using ShootingGame.Data;
using System;
namespace ShootingGame
{   
    public class PlayerDefender : ADefender
    {
        [SerializeField] private float _timeInvulnerability = 1f;
        [SerializeField] private ProgressBar healthBar;
        private bool _invulnerability;
        private Action OnDefendAction;

        public Action OnDeadAction;

        private float dodgeChance;
        private int armor;

        private PlayerStat playerStat;

        public override void Defend(Interface.IAttacker attacker, bool isSuper = false,  (float, Transform) forceProp = default)
        {
            if (_invulnerability) return;
            UpdateStatsFromEquipment();
            if (UnityEngine.Random.value < dodgeChance) return;
            _invulnerability = true;
            Invoke(nameof(ResetInvulnerability), _timeInvulnerability);
            int damageAfterArmor = Mathf.Max(attacker.Damage - armor, 0);
            Defend(damageAfterArmor);
            if(!IsDead) OnDefendAction?.Invoke();
            else OnDead(attacker);
        }

        private void ResetInvulnerability() => _invulnerability = false;

        public override void OnDead(Interface.IAttacker attacker) => OnDeadAction?.Invoke();

        internal void Init(PlayerStat playerStat, Action OnDefendAction = null, Action OnDeadAction = null)
        {
            this.playerStat = playerStat;
            SetHealth((int)playerStat.CurrentStat.GetStat(TypeStat.Hp).Value, true);
            this.OnDeadAction = OnDeadAction;
            this.OnDefendAction += () =>
            {
                OnDefendAction?.Invoke();
                healthBar?.UpdateProgess(CurrentHealth, MaxHealth);
            };
        }

        public override void SetHealth(int health, bool resetHealth = true)
        {
            base.SetHealth(health, resetHealth);
            healthBar?.UpdateProgess(CurrentHealth, MaxHealth);
        }

        public override void BuffHealth(int value)
        {
            base.BuffHealth(value);
            healthBar?.UpdateProgess(CurrentHealth, MaxHealth);
        }


        private void UpdateStatsFromEquipment()
        {
            if (playerStat != null)
            {
                var currentStat = playerStat.CurrentStat;

                var dodgeStats = currentStat.GetStat(TypeStat.Dodge);
                dodgeChance = (dodgeStats.TypeStat == TypeStat.Dodge) ? dodgeStats.GetValue() : 0f;

                var armorStats = currentStat.GetStat(TypeStat.Armor);
                armor = (armorStats.TypeStat == TypeStat.Armor) ? (int)armorStats.GetValue(armor) : 0;

                //Debug.Log($"PlayerDefender: UpdateStatsFromEquipment dodgeChance: {dodgeChance} armor: {armor}");
            }
        }

    }
}
