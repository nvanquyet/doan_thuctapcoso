using System;
using UnityEngine;

namespace ShootingGame
{
    public class EnemyDefender : ADefender
    {
        [SerializeField] ColoredFlash flash;

        public Action OnDefend;
        public Action OnDefendSuccess;
        public override void ExitInteract(Interface.Interact target) { }

        public override void Interact(Interface.Interact target) { }

        public override void OnDead()
        {
            
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
    }
}
