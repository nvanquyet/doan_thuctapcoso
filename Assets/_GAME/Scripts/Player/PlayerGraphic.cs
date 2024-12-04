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
        private void Start()
        {
            SetAnimator(GameData.Instance.Players.GetValue(UserData.CurrentCharacter).Animator);
        }
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

        public void SetAnimator(RuntimeAnimatorController runtimeAnimatorController)
        {
            Animator.runtimeAnimatorController = runtimeAnimatorController;
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