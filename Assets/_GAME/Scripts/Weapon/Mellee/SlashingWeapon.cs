using DG.Tweening;
using UnityEngine;
namespace ShootingGame
{

    public class SlashingWeapon : MelleeWeapon
    {
        [System.Serializable]
        private struct TweenRotationStruct
        {
            public float windUpRatio;
            public float slashRatio;
            public float recoverRatio;
            public Vector3 windUpAngle;
            public Vector3 slashAngle;
            public Vector3 recoverAngle;
        }
        [SerializeField] private TweenRotationStruct tweenRotationStruct;
        private (float, float, float) slashingTime; 

       

        private void Start()
        {
            OnAttackSpeedChange();
        }

        protected override void OnAttackSpeedChange()
        {
            slashingTime.Item1 = attackSpeed * tweenRotationStruct.windUpRatio;
            slashingTime.Item2 = attackSpeed * tweenRotationStruct.slashRatio;
            slashingTime.Item3 = attackSpeed * tweenRotationStruct.recoverRatio;
        }

        protected override void CreateTweenSequence()
        {
            Sequence attackSequence = DOTween.Sequence();
            attackSequence.AppendCallback(() => EnableInteract(true));
            //Wind-up
            attackSequence.Append(WeaponTs.DOLocalRotate(tweenRotationStruct.windUpAngle, slashingTime.Item1)
                .SetEase(Ease.InOutQuad).From(tweenRotationStruct.recoverAngle));
                
            //Slash
            attackSequence.Append(WeaponTs.DOLocalRotate(tweenRotationStruct.slashAngle, slashingTime.Item2)
                .SetEase(Ease.Linear).From(tweenRotationStruct.windUpAngle));

            //Recover
            attackSequence.Append(WeaponTs.DOLocalRotate(tweenRotationStruct.recoverAngle, slashingTime.Item3)
                .SetEase(Ease.OutQuad).From(tweenRotationStruct.slashAngle).OnComplete(() => EnableInteract(false)));

            attackSequence.Play();
        }

    }

}