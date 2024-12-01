using UnityEngine;

public class MeleeAttack : AttackBehaviour
{
    public override void ExecuteAttack()
    {
        if (target is MonoBehaviour)
        {
            if(Vector2.Distance((target as MonoBehaviour).transform.position, transform.position) <= (AttackRange * 1.5f))
            {
                target.Defend(impactData.damage, impactData.isCritical, (impactData.pushForce, transform));
            }
        }
    }
}
