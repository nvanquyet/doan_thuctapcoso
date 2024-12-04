using System;
using UnityEngine;

namespace ShootingGame
{
    public class EnemyDefender : ADefender
    {
        public Action OnDefend;
        public Action OnDefendSuccess;
        public Action OnDeath;
        
        public override void OnDead() {
            OnDeath?.Invoke();
        }

        public override void Defend(int damage, bool isSuper = false, (float, Transform) forceProp = default)
        {
            base.Defend(damage, isSuper, forceProp);
            OnDefend?.Invoke();
            Invoke(nameof(DefendSuccess), .25f);
        }

        private void DefendSuccess()
        {
            Rigid.velocity = Vector2.zero;
            OnDefendSuccess?.Invoke();
        }

        internal void Init(float scaleFactor)
        {
            SetHealth((int)(scaleFactor * MaxHealth));
        }
    }
}
