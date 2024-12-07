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
            OnDefendAction?.Invoke();
        }

        private void ResetInvulnerability() => _invulnerability = false;

        public override void OnDead(Interface.IAttacker attacker) { 
            this.Dispatch<GameEvent.OnPlayerDead>();
        }

        internal void Init(PlayerStat playerStat, Action OnDefendAction = null)
        {
            this.playerStat = playerStat;
            SetHealth((int)playerStat.CurrentStat.GetStat(TypeStat.Hp).Value, true);
            this.OnDefendAction += () =>
            {
                OnDefendAction?.Invoke();
                healthBar?.UpdateProgess(CurrentHealth, MaxHealth);
            };
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
