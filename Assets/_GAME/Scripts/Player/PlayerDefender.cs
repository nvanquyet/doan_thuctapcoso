using System;
using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{   
    public class PlayerDefender : ADefender
    {
        [SerializeField] private ColoredFlash _flash;
        [SerializeField] private float _timeInvulnerability = 1f;
        private bool _invulnerability;

        private float dodgeChance;
        private int armor;

        [SerializeField] private PlayerStat playerStat;

        public override void Defend(int damage)
        {
            if (_invulnerability) return;
            UpdateStatsFromEquipment();

            if (UnityEngine.Random.value < dodgeChance)
            {
                Debug.Log("Attack dodged!");
                return;
            }

            _invulnerability = true;
            Invoke(nameof(ResetInvulnerability), _timeInvulnerability);
            if (_flash != null) _flash.Flash(Color.white);


            int damageAfterArmor = Mathf.Max(damage - armor, 0);
            base.Defend(damageAfterArmor);
        }

        private void ResetInvulnerability() => _invulnerability = false;

        public override void OnDead() { }

        internal void Init()
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null && _flash != null) _flash.SetSpriteRenderer(sprite);
        }



        private void UpdateStatsFromEquipment()
        {
            if (playerStat != null)
            {
                var currentStat = playerStat.CurrentStat;

                var dodgeStats = currentStat.Data.GetStat(TypeStat.Dodge);
                dodgeChance = (dodgeStats.TypeStat == TypeStat.Dodge) ? dodgeStats.GetValue(1f) : 0f;

                var armorStats = currentStat.Data.GetStat(TypeStat.Armor);
                armor = (armorStats.TypeStat == TypeStat.Armor) ? (int)armorStats.GetValue(armor) : 0;
            }
        }

    }
}
