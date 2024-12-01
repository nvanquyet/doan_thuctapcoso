using ShootingGame;
using System;
using System.Collections;
using UnityEngine;
using static ShootingGame.Interface;

public class AttackController : AAttacker, IAttackBehaviour
{
    private Interface.IDefender _currentTarget;
    private bool _isAttacking = false;

    [SerializeField] private AttackBehaviour[] attackBehaviours;
    [SerializeField] private TriggerAnimation triggerAnimation;
    [SerializeField] private int timeChangeBehaviour = 5;

    private IDefender defenderOwner;
    private ADefender _target;

    private Coroutine attackRoutine;

    private float distanceAttack = -1;
    public float DistanceAttack
    {
        get
        {
            if(distanceAttack <= 0) distanceAttack = attackBehaviours[0].AttackRange;
            return distanceAttack;
        }
    }

    private Action OnCompleteAction;
    private void Start()
    {
        defenderOwner = GetComponentInParent<IDefender>();
        triggerAnimation.OnTriggerAction += OnTriggerAnimation;
        SetTarget(_target);
    }



#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        attackBehaviours = GetComponentsInChildren<AttackBehaviour>();
        triggerAnimation = GetComponentInChildren<TriggerAnimation>();
    }
#endif


    public void OnTriggerAnimation(string nameAnimaiton)
    {
        GameService.LogColor("Trigger animation: " + nameAnimaiton);
        foreach (var b in attackBehaviours)
        {
            if (b.TriggerAnimation == nameAnimaiton)
            {
                b.ExecuteAttack();
                return;
            }
        }
        OnCompleteAction?.Invoke();
    }

    public void SetTarget(ADefender target)
    {
        this._target = target;
        if (attackBehaviours != null && attackBehaviours.Length > 0)
        {
            foreach (var b in attackBehaviours)
            {
                b.Initialize(defenderOwner, _target);
            }
        }
    }
    private void OnEnable()
    {
        ExecuteAttack();
    }
    public void ExecuteAttack()
    {
        if(attackRoutine != null) StopCoroutine(attackRoutine);
        attackRoutine = StartCoroutine(IEAttackRoutine());
    }

    private void OnDisable()
    {
        if (attackRoutine != null) StopCoroutine(attackRoutine);
        attackRoutine = null;
    }

    private IEnumerator IEAttackRoutine()
    {
        yield return new WaitForSeconds(timeChangeBehaviour);
        while (true)
        {
            var randomBehaviour = UnityEngine.Random.Range(0, attackBehaviours.Length);
            var behaviour = attackBehaviours[randomBehaviour];  
            distanceAttack = behaviour.AttackRange;
            var impactData = new ImpactData(Damage, false, 0);
            if(behaviour is MeleeAttack)
            {
                if (Vector2.Distance(transform.position, _target.transform.position) <= DistanceAttack)
                {
                    attackBehaviours[randomBehaviour].InitializeImpactData(impactData);
                }
            } else attackBehaviours[randomBehaviour].InitializeImpactData(impactData);

            yield return new WaitForSeconds(timeChangeBehaviour);
        }
    }

    public void SetAttackAction(Action onStartAction, Action onCompleteAction)
    {
        if(attackBehaviours == null || attackBehaviours.Length == 0) return;
        onStartAction += () => _isAttacking = true;
        onCompleteAction += () => _isAttacking = false;
        foreach (var b in attackBehaviours)
        {
            b.OnStartAttack = onStartAction;
        }
        OnCompleteAction = onCompleteAction;
    }

    public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
    {
        if(!_isAttacking) return false;
        if (target == null || target.GetType().Equals(defenderOwner)) return false;
        return base.Attack(target, isSuper, forcePushBack);
    }

    public void Init(float scaleFactor)
    {
        SetDamage((int)(scaleFactor * Damage));
    }

    public override void ExitInteract(Interface.IInteract target) { }
    public override void OnInteract(Interface.IInteract target) { }
    protected override void OnTriggerEnter2D(Collider2D other) { }

    protected override void OnTriggerExit2D(Collider2D other) { }

   
}
