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

        protected override void OnAttackSpeedChange()
        {
            base.OnAttackSpeedChange();
            var atk = attackSpeed / 2;
            slashingTime.Item1 = atk * tweenRotationStruct.windUpRatio;
            slashingTime.Item2 = atk * tweenRotationStruct.slashRatio;
            slashingTime.Item3 = atk * tweenRotationStruct.recoverRatio;
        }

        protected override void CreateTweenSequence()
        {
            Sequence attackSequence = DOTween.Sequence();
            attackSequence.AppendCallback(() => EnableInteract(true));
            var rangeMultiplier = CurrentEquiqmentStat.GetStat(Data.TypeStat.RangeWeapon).Value;
            GameService.LogColor($"Range Multiplier: {rangeMultiplier}");
            Vector3 extendedWindUpAngle = tweenRotationStruct.windUpAngle * rangeMultiplier;
            Vector3 extendedSlashAngle = tweenRotationStruct.slashAngle * rangeMultiplier;
            Vector3 extendedRecoverAngle = tweenRotationStruct.recoverAngle * rangeMultiplier;

            // Wind-up
            attackSequence.Append(WeaponTs.DOLocalRotate(extendedWindUpAngle, slashingTime.Item1)
                .SetEase(Ease.InOutQuad).From(tweenRotationStruct.recoverAngle));

            // Slash
            attackSequence.Append(WeaponTs.DOLocalRotate(extendedSlashAngle, slashingTime.Item2)
                .SetEase(Ease.Linear).From(tweenRotationStruct.windUpAngle));

            // Recover
            attackSequence.Append(WeaponTs.DOLocalRotate(extendedRecoverAngle, slashingTime.Item3)
                .SetEase(Ease.OutQuad).From(tweenRotationStruct.slashAngle).OnComplete(() => EnableInteract(false)));

            attackSequence.Play();
        }

    }

}