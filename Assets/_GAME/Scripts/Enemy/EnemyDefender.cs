using System;
using UnityEngine;

namespace ShootingGame
{
    public class EnemyDefender : ADefender
    {
        public Action<float> OnDefend;
        public Action OnDefendSuccess;
        public Action<Interface.IAttacker> OnDeath;

        public override void OnDead(Interface.IAttacker attacker)
        {
            base.OnDead(attacker);
            OnDeath?.Invoke(attacker);
        }

        public override void Defend(Interface.IAttacker attacker, bool isSuper = false, (float, Transform) forceProp = default)
        {
            base.Defend(attacker, isSuper, forceProp);
            OnDefend?.Invoke((float) CurrentHealth * 1.0f/ MaxHealth);
            Invoke(nameof(DefendSuccess), .25f);
        }

        private void DefendSuccess()
        {
            Rigid.velocity = Vector2.zero;
            OnDefendSuccess?.Invoke();
        }

        internal void Init(int health, int expGiven, int coinGiven, float growthRate)
        {
            SetHealth((int)(growthRate * health));
            this.expGiven = (int)(expGiven * growthRate);
            this.coinGiven = (int)(coinGiven * growthRate);
        }
    }
}
