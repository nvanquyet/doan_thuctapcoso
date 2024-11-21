
using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
namespace ShootingGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour, Interface.IPlayerMovement
    {
        [SerializeField] protected float _repeatTimeUpdatePath = 0.5f;
        [SerializeField] protected float _moveSpeed = 2f;
        [SerializeField] protected float _nextWayPointDistance = .2f;

        protected Seeker _seeker;
        protected Rigidbody2D _rb;
        protected Path _path;
        protected Transform _target;
        protected Coroutine _moveCoroutine;
        protected SpriteRenderer _characterSR;

        private float attackRange;

        public Action OnRandomTarget;

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
        public void SetAttackRange(float value) => attackRange = value;
        protected void GetTarget() => OnRandomTarget?.Invoke();

        public void SetTarget(Transform target) => _target = target;

        protected void CalculatePath()
        {
            if (_target == null)
            {
                GetTarget();
                return;
            }
            if (_seeker.IsDone()) _seeker.StartPath(_rb.position, _target.position, OnPathCompleted);
        }


        protected void OnPathCompleted(Path p)
        {
            if (!p.error)
            {
                _path = p;
                Move(Vector3.zero);
            }
        }

        protected virtual IEnumerator MoveToTargetCoroutine()
        {
            int currentWP = 0;
            while (_path != null && currentWP < _path.vectorPath.Count)
            {

                if(_target == null) GetTarget();
                float distance = Vector2.Distance(_rb.position, _path.vectorPath[currentWP]);
                Vector2 direction = ((Vector2)_path.vectorPath[currentWP] - _rb.position).normalized;
                Vector2 force = direction * _moveSpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, _target.transform.position) >= attackRange)
                {
                    transform.position += (Vector3)force;
                    if (distance < _nextWayPointDistance)
                        currentWP++;
                }
                
                if (force.x != 0){
                    var scaleSprite = Mathf.Abs(_characterSR.transform.localScale.x);
                    if (force.x < 0)
                        _characterSR.transform.localScale = new Vector3(1, 1, 0) * scaleSprite;
                    else
                        _characterSR.transform.localScale = new Vector3(-1, 1, 0) * scaleSprite;
                }
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

        internal void Init(float scaleFactor) { }
        
    }
}