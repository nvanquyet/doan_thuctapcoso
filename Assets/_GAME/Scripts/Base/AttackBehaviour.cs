using ShootingGame;
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

public abstract class AttackBehaviour : MonoBehaviour, IAttackBehaviour
{
    [SerializeField] private string triggerAnimation = "Attack";
    [SerializeField] private float attackRange = 1.5f;

    public float AttackRange => attackRange;
    public string TriggerAnimation => triggerAnimation;
    public Animator Animator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            return animator;
        }
    }
    public Action OnStartAttack;

    protected Animator animator;
    protected IDefender owner;
    protected IDefender target;

    protected ImpactData impactData;

    public void InitializeImpactData(ImpactData impactData)
    {
        this.impactData = impactData;
        OnTriggerAnimation();
    }

    public virtual void Initialize(IDefender owner, IDefender target)
    {
        this.owner = owner;
        this.target = target;
    }
    protected virtual void OnTriggerAnimation()
    {
        OnStartAttack?.Invoke();
        Animator?.SetTrigger(triggerAnimation);
    }
    public abstract void ExecuteAttack();

    protected float GetAnimationDuration()
    {
        var clips = Animator.runtimeAnimatorController.animationClips;

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
