using DG.Tweening;
using UnityEngine;

public class RollAttack : AttackBehaviour
{
    [SerializeField] private float distanceRoll;

    protected override void OnTriggerAnimation()
    {
        base.OnTriggerAnimation();
        Animator?.SetBool("IsRolling", true);
        if (target is MonoBehaviour)
        {
            var direction = (Vector2)((target as MonoBehaviour).transform.position - transform.position).normalized;
            this.transform.DOMove((Vector2)transform.position + direction.normalized * distanceRoll, 1.24f).SetEase(Ease.Linear).SetDelay(1.292f)
                .OnComplete(() =>
                {
                    Animator?.SetBool("IsRolling", false);
                });
        }
    }

    public override void ExecuteAttack()
    {
       
    }
}
