using System;
using UnityEngine;

namespace ShootingGame
{
    public class EnemyDefender : ADefender
    {
        [SerializeField] ColoredFlash flash;

        public Action OnDefend;
        public Action OnDefendSuccess;
        public Action OnDeath;
        
        public override void OnDead() {
            OnDeath?.Invoke();
        }

        public override void Defend(int damage)
        {
            base.Defend(damage);
            // Flash
            if (flash != null)  flash.Flash(Color.white);
            OnDefend?.Invoke();
            Invoke(nameof(DefendSuccess), 1f);
        }

        private void DefendSuccess() => OnDefendSuccess?.Invoke();

        internal void Init(float scaleFactor)
        {
            SetHealth((int)(scaleFactor * MaxHealth));
        }
    }
}
