
using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
namespace ShootingGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour, Interface.IPlayerMovement
    {
        [SerializeField] private float _repeatTimeUpdatePath = 0.5f;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _nextWayPointDistance = .2f;

        private Seeker _seeker;
        private Rigidbody2D _rb;

        private Path _path;
        private Transform _target;
        private Coroutine _moveCoroutine; 
        private SpriteRenderer _characterSR;

        public PlayerLocoMotionState LocomotionState => PlayerLocoMotionState.Run;

        public bool IsMoving => CurrentSpeed > 0;

        public float Speed => _moveSpeed;

        public float CurrentSpeed => _rb == null ? 0 : _rb.velocity.magnitude;

        private void Start()
        {
            _seeker = GetComponent<Seeker>();
            _rb = GetComponent<Rigidbody2D>();
            _characterSR = GetComponentInChildren<SpriteRenderer>();
            InvokeRepeating(nameof(CalculatePath), 0f, _repeatTimeUpdatePath);
        }

        private Transform GetTarget()
        {
            if (_target == null) _target = GameCtrl.Instance.GetRandomTransformPlayer();
            return _target;
        }

        void CalculatePath()
        {
            if (_target == null)
            {
                GetTarget();
                return;
            }
            if (_seeker.IsDone())
                _seeker.StartPath(_rb.position, _target.position, OnPathCompleted);
        }


        void OnPathCompleted(Path p)
        {
            if (!p.error)
            {
                _path = p;
                Move(Vector3.zero);
            }
        }

        IEnumerator MoveToTargetCoroutine()
        {
            int currentWP = 0;
            while (currentWP < _path.vectorPath.Count)
            {

                if(_target == null) GetTarget();

                Vector2 direction = ((Vector2)_path.vectorPath[currentWP] - _rb.position).normalized;
                Vector2 force = direction * _moveSpeed * Time.deltaTime;
                transform.position += (Vector3)force;

                float distance = Vector2.Distance(_rb.position, _path.vectorPath[currentWP]);
                if (distance < _nextWayPointDistance)
                    currentWP++;

                if (force.x != 0)
                    if (force.x < 0)
                        _characterSR.transform.localScale = new Vector3(1, 1, 0);
                    else
                        _characterSR.transform.localScale = new Vector3(-1, 1, 0);

                yield return null;
            }
        }

        public void Stop()
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        public void Continue()
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
        }

        public void PauseMovement(bool pauseMovement) {
            if(pauseMovement) Stop();
            else Continue();
        } 

        public void SetSpeed(float speed) => _moveSpeed = Mathf.Max(speed, 1);

        public void Move(Vector3 direction) => Continue();

        internal void Init(float scaleFactor)
        {
            throw new NotImplementedException();
        }
    }
}