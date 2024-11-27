using UnityEngine;
namespace ShootingGame
{

    [RequireComponent(typeof(Animator))]
    public class PlayerGraphic : MonoBehaviour, Interface.IPlayerGraphic
    {
        #region Properties
        private Animator _animator;

        private int _locomotionAnimation;
        private int _hitAnimation; 

        private Interface.IPlayerMovement playerMovement;

        #endregion  

        #region Implement
        public Animator Animator {
            get {
                if (_animator == null) _animator = GetComponent<Animator>();
                return _animator;
            }
        }

        public float Speed => playerMovement == null ? 0 : playerMovement.CurrentSpeed;

        public void HashString()
        {
            _locomotionAnimation = Animator.StringToHash("Locomotion");
            _hitAnimation = Animator.StringToHash("OnHit");
        }

        public void SetAnimation(RuntimeAnimatorController runtimeAnimatorController)
        {
            _animator.runtimeAnimatorController = runtimeAnimatorController;
        }

        public void SetLocoMotionState(PlayerLocoMotionState state)
        {
            switch(state)
            {
                case PlayerLocoMotionState.Idle:
                    Animator.SetFloat(_locomotionAnimation, 0);
                    break;
                case PlayerLocoMotionState.Run:
                    
                    Animator.SetFloat(_locomotionAnimation, Speed);
                    break;
            }
        }
        #endregion
       
       public void Init(Interface.IPlayerMovement playerMovement){
            this.playerMovement = playerMovement;
            HashString();
       }

        public void OnHitAnimation()
        {
            Animator.SetTrigger(_hitAnimation);
        }

        private void LateUpdate()
        {
            if(playerMovement != null) SetLocoMotionState(playerMovement.LocomotionState);
        }

    }
}