
using UnityEngine;

namespace ShootingGame
{

    public partial interface Interface
    {

        public interface IPlayerGraphic
        {
            Animator Animator { get; }

            float Speed { get; }

            void SetAnimator(RuntimeAnimatorController runtimeAnimatorController);

            void SetLocoMotionState(PlayerLocoMotionState state);

            void HashString();
        }



        public interface IPlayerMovement : Interface.IMoveable
        {
            PlayerLocoMotionState LocomotionState { get; }
            void Stop();
            void Continue();
            void PauseMovement(bool pauseMovement);
            bool IsMoving { get; }
            float Speed { get; }
            float CurrentSpeed { get; }
            void SetSpeed(float speed);
        }



        public interface IPlayerSpawner : Interface.ISpawner
        {
            PlayerGraphic PlayerGraphic { get; }
        }

    }

}