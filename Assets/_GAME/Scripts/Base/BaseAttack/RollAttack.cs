using DG.Tweening;
using UnityEngine;

public class RollAttack : AttackBehaviour
{
    [SerializeField] private float distanceRoll;
    
    public override void ExecuteAttack(Vector2 direction, ImpactData param)
    {
        this.transform.DOMove((Vector2)transform.position + direction.normalized * distanceRoll, GetAnimationDuration()).SetEase(Ease.Linear).SetDelay(0.05f);
        base.ExecuteAttack(direction, param);
    }
}
