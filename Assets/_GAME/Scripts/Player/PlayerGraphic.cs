using UnityEngine;
namespace ShootingGame
{
    public interface IPlayerGraphic
    {
        Animator Animator { get; }

        float Speed { get; }

        void SetAnimation(RuntimeAnimatorController runtimeAnimatorController);

        void SetLocoMotionState(PlayerLocoMotionState state);

        void HashString();
    }



    [RequireComponent(typeof(Animator))]
    public class PlayerGraphic : MonoBehaviour, IPlayerGraphic
    {
        #region Properties
        private Animator _animator;

        private int _locomotionAnimation;

        private IPlayerMovement playerMovement;

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
       
       public void Init(IPlayerMovement playerMovement){
            this.playerMovement = playerMovement;
            HashString();
       }

        private void LateUpdate()
        {
            SetLocoMotionState(playerMovement.LocomotionState);
        }

    }
}