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
        }
        [SerializeField] private TweenRotationStruct tweenRotationStruct;
        [SerializeField] private DOTweenAnimation windUpTweenAnimation;
        [SerializeField] private DOTweenAnimation slashTweenAnimation;
        [SerializeField] private DOTweenAnimation recoverTweenAnimation;

        private void Start(){
            slashTweenAnimation.onComplete.AddListener(recoverTweenAnimation.DOPlay);
            windUpTweenAnimation.onComplete.AddListener(slashTweenAnimation.DOPlay);
            OnAttackSpeedChange();
        }

        public override bool Attack()
        {
            if(base.Attack()) { 
                windUpTweenAnimation.DOPlay();
                return true;
            }
            return false;
        }

        protected override void OnAttackSpeedChange()
        {
            windUpTweenAnimation.duration = attackSpeed * tweenRotationStruct.windUpRatio;
            slashTweenAnimation.duration = attackSpeed * tweenRotationStruct.slashRatio;
            recoverTweenAnimation.duration = attackSpeed * tweenRotationStruct.recoverRatio;
        }
    }

}