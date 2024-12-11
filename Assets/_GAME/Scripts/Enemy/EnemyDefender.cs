using System;
using UnityEngine;

namespace ShootingGame
{
    public class EnemyDefender : ADefender
    {
        public Action OnDefend;
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
            OnDefend?.Invoke();
            Invoke(nameof(DefendSuccess), .25f);
        }

        private void DefendSuccess()
        {
            Rigid.velocity = Vector2.zero;
            OnDefendSuccess?.Invoke();
        }

        internal void Init(int health, int expGiven, float growthRate)
        {
            SetHealth((int)(growthRate * health));
            this.expGiven = (int)(expGiven * growthRate);
        }
    }
}
