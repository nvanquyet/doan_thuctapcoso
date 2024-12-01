using DG.Tweening;
using UnityEngine;

public class RollAttack : AttackBehaviour
{
    [SerializeField] private float distanceRoll;
    
    public override void ExecuteAttack()
    {
        if(target is MonoBehaviour)
        {
            var direction = (Vector2) ((target as MonoBehaviour).transform.position - transform.position).normalized;
            this.transform.DOMove((Vector2)transform.position + direction.normalized * distanceRoll, GetAnimationDuration()).SetEase(Ease.Linear).SetDelay(0.05f);
        }
    }
}
