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

        private PlayerStat playerStat;

        public override void Defend(int damage, bool isSuper = false,  (float, Transform) forceProp = default)
        {
            if (_invulnerability) return;
            UpdateStatsFromEquipment();
            if (UnityEngine.Random.value < dodgeChance) return;

            _invulnerability = true;
            Invoke(nameof(ResetInvulnerability), _timeInvulnerability);
            if (_flash != null) _flash.Flash(Color.white);

            int damageAfterArmor = Mathf.Max(damage - armor, 0);
            base.Defend(damageAfterArmor, isSuper, forceProp);

            Test();
        }

        private void ResetInvulnerability() => _invulnerability = false;

        public override void OnDead() { 
            this.Dispatch<GameEvent.OnPlayerDead>();
        }

        internal void Init(PlayerStat playerStat)
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null && _flash != null) _flash.SetSpriteRenderer(sprite);
            this.playerStat = playerStat;
            SetHealth((int)playerStat.CurrentStat.GetStat(TypeStat.Hp).Value, true);
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


        /// <summary>
        /// Remove this method after testing
        /// </summary>
        private void Test(){
            var healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null)
            {
                healthBar.UpdateHealth(CurrentHealth, MaxHealth);
            }
        }

    }
}
