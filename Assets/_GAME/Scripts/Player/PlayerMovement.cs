using System;
using UnityEngine;
namespace ShootingGame
{
    public enum PlayerLocoMotionState
    {
        Idle,
        Run
    }


    [RequireComponent(typeof(RigidbodyType2D))]
    public class PlayerMovement : MonoBehaviour, Interface.IPlayerMovement
    {
        #region Properties
        [SerializeField] private float accelerationTime = 0.1f;

        private float _speed;
        private Transform _characterGraphic;
        private Rigidbody2D _rigid;
        private PlayerLocoMotionState _locomotionState;
        private bool _paused;
        private Vector3 movementInput;
        private Vector2 currentVelocity;
        #endregion

        public void Init(Transform characterGraphic)
        {
            _characterGraphic = characterGraphic;
        }

        #region  Implement  

        public bool IsMoving => _speed > 0 && !_paused;

        public float Speed => _speed;

        public float CurrentSpeed => Rigid == null ? 0 : Rigid.velocity.magnitude;


        public Rigidbody2D Rigid
        {
            get
            {
                if (_rigid == null) _rigid = GetComponent<Rigidbody2D>();
                return _rigid;
            }
        }
        public PlayerLocoMotionState LocomotionState => _locomotionState;

        public void PauseMovement(bool pauseMovement) => _paused = pauseMovement;
        public void SetSpeed(float speed) => this._speed = Mathf.Max(speed, 1);
        public void Stop() => PauseMovement(true);
        public void Continue() => PauseMovement(false);

        #endregion

        #region  Movement Update
        private void Update()
        {
            if (IsMoving)
            {
                movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
                if (movementInput.x != 0)
                {
                    Vector3 newScale = _characterGraphic.localScale;
                    if (movementInput.x < 0)
                        newScale.x = -1 * Mathf.Abs(newScale.x);
                    else
                        newScale.x = Mathf.Abs(newScale.x);
                    _characterGraphic.localScale = newScale;
                }

                if (movementInput.magnitude > 0)
                {
                    _locomotionState = PlayerLocoMotionState.Run;
                }
                else
                {
                    _locomotionState = PlayerLocoMotionState.Idle;
                }
            }
            else
            {
                _locomotionState = PlayerLocoMotionState.Idle;
            }
        }

        private void FixedUpdate()
        {
            if (IsMoving)
            {
                Move(movementInput);
            }
        }

        public void Move(Vector3 direction)
        {
            Vector2 targetVelocity = direction.normalized * Speed;
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accelerationTime / Time.fixedDeltaTime);
            Rigid.velocity = currentVelocity;
        }



        #endregion
    }

}