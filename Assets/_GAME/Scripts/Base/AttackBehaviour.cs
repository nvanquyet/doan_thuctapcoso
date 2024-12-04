using System;
using UnityEngine;
using static ShootingGame.Interface;

[System.Serializable]
public struct ImpactData
{
    public int damage;

    public bool isCritical;

    public float pushForce;

    public ImpactData(int damage, bool isCritical, float pushForce)
    {
        this.damage = damage;
        this.isCritical = isCritical;
        this.pushForce = pushForce;
    }
}

public class AttackBehaviour : MonoBehaviour, IAttackBehaviour
{
    [SerializeField] private string triggerAnimation = "Attack";

    public Action OnStartAttack;

    public Action OnEndAttack;
    protected Animator animator;
    protected IDefender defenderOwner;


    private int hashTriggerAnimation;

    public virtual void Initialize(Animator animator, IDefender owner)
    {
        this.animator = animator;
        this.defenderOwner = owner;
        hashTriggerAnimation = Animator.StringToHash(triggerAnimation);
    }

    public virtual void ExecuteAttack(Vector2 direction, ImpactData param)
    {
        animator?.SetTrigger(hashTriggerAnimation);
    }

    protected float GetAnimationDuration()
    {
        var clips = animator.runtimeAnimatorController.animationClips;

        foreach (var clip in clips)
        {
            if (clip.name == triggerAnimation)
            {
                return clip.length;
            }
        }
        return 0;
    }
}
