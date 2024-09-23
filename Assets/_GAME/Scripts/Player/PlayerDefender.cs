using UnityEngine;

namespace ShootingGame
{
    public class PlayerDefender : ADefender
    {
        [SerializeField] private ColoredFlash _flash;
        [SerializeField] private float _timeInvulnerability = 1f;
        private bool _invulnerability;

        public override void Defend(int damage)
        {
            if (_invulnerability) return;
            _invulnerability = true;
            Invoke(nameof(ResetInvulnerability), _timeInvulnerability);
            if (_flash != null) _flash.Flash(Color.white);
            base.Defend(damage);
        }

        private void ResetInvulnerability() => _invulnerability = false;

        public override void OnDead()
        {
            FindObjectOfType<LosePanel>().Show();
        }

        internal void Init()
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null) _flash.SetSpriteRenderer(sprite);
            SetHealth(MaxHealth, true);
        }
        
    }

}
