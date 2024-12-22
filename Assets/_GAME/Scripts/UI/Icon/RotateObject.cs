using DG.Tweening;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Transform tweenTarget;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Vector3 startRotation = new Vector3(0, 0, 90);
    [SerializeField] private Vector3 endRotation = new Vector3(0, 0, -90);
    [SerializeField] private LoopType loopType = LoopType.Incremental;
    [SerializeField] private RotateMode rotateMode = RotateMode.FastBeyond360;
    [SerializeField] private int loops = -1;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private bool isLocal = true;

    private Tween tween;
    private Transform TweenTarget
    {

        get => tweenTarget == null ? transform : tweenTarget;
    }

    private void OnEnable()
    {
        if (isLocal)
        {
            tween = TweenTarget.DOLocalRotate(endRotation, duration, rotateMode).SetLoops(loops, loopType).SetEase(ease).From(startRotation).SetDelay(0.2f);
        }
        else
        {
            tween = TweenTarget.DORotate(endRotation, duration, rotateMode).SetLoops(loops, loopType).SetEase(ease).From(startRotation).SetDelay(0.2f);
        }

    }

    private void OnDisable()
    { 
        tween.Kill();
    }
}
