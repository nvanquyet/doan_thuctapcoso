using DG.Tweening;

namespace ShootingGame
{
    public class ThrustingWeapon : MelleeWeapon
    {

        protected override void CreateTweenSequence()
        {
            Sequence attackSequence = DOTween.Sequence();
            attackSequence.AppendCallback(() => EnableInteract(true));
            var timeAvgAttack = attackSpeed / 2;
            attackSequence.Append(WeaponTs.DOLocalMoveX(CurrentEquiqmentStat.GetStat(Data.TypeStat.RangeWeapon).Value, timeAvgAttack / 2).SetEase(Ease.InOutQuad));
            attackSequence.Append(WeaponTs.DOLocalMoveX(0, timeAvgAttack / 2).SetEase(Ease.InOutQuad).OnComplete(() => EnableInteract(false)));
            attackSequence.Play();
        }

    }
}
