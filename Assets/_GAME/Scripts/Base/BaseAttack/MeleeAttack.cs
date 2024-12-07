using UnityEngine;
using static ShootingGame.Interface;

public class MeleeAttack : AttackBehaviour, IAttacker
{
    public int Damage => impactData.damage;

    public bool CanAttack => true;

    public Collider2D Collider => null;

    public bool Attack(IDefender target, bool isSuper = false, float forcePushBack = 0) => false;

    public override void ExecuteAttack()
    {
        if (target is MonoBehaviour)
        {
            if(Vector2.Distance((target as MonoBehaviour).transform.position, transform.position) <= (AttackRange * 1.5f))
            {
                target.Defend(this, impactData.isCritical, (impactData.pushForce, transform));
            }
        }
    }

    public void ExitInteract(IInteract target) { }

    public void GainExp(int exp) { }

    public void OnInteract(IInteract target) { }

    public void SetCanAttack(bool value) { }

    public void SetDamage(int damage) { }
}
