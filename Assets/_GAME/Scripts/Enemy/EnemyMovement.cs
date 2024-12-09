
using System;
using System.Collections;
using Mono.CSharp;
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
        protected Path _path;
        protected Transform _target;
        protected Coroutine _moveCoroutine;
        protected SpriteRenderer _characterSR;

        private float attackRange;

        public Action OnRandomTarget;

        public Action<float> OnMoveAction;

        public PlayerLocoMotionState LocomotionState => PlayerLocoMotionState.Run;

        public bool IsMoving => CurrentSpeed > 0;

        public float Speed => _moveSpeed;

        public float CurrentSpeed => 0;

        private void Start()
        {
            _seeker = GetComponent<Seeker>();
            _characterSR = GetComponentInChildren<SpriteRenderer>();
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
            if (_seeker.IsDone()) _seeker.StartPath(transform.position, _target.position, OnPathCompleted);
        }


        protected void OnPathCompleted(Path p)
        {
            if (!p.error) _path = p;
        }

        protected virtual IEnumerator MoveToTargetCoroutine()
        {
            int currentWP = 0;

            yield return new WaitUntil(() => _path != null);
            while (_path != null)
            {
                if(currentWP >= _path.vectorPath.Count)
                {
                    CalculatePath();
                    yield return null;
                    currentWP = 0;
                    continue;
                }
                if (_target == null) GetTarget();
                float distance = Vector2.Distance(transform.position, _path.vectorPath[currentWP]);
                Vector2 direction = (Vector2)(_path.vectorPath[currentWP] - transform.position).normalized;
                Vector2 force = Vector2.zero;
                if (Vector3.Distance(transform.position, _target.transform.position) >= attackRange)
                {
                    force = direction * _moveSpeed * Time.deltaTime;
                    transform.position += (Vector3)force;
                    if (distance < _nextWayPointDistance)
                        currentWP++;
                }

                if (force.x != 0)
                {
                    var scaleSprite = Mathf.Abs(_characterSR.transform.localScale.x);
                    if (force.x < 0)
                        _characterSR.transform.localScale = new Vector3(1, 1, 0) * scaleSprite;
                    else
                        _characterSR.transform.localScale = new Vector3(-1, 1, 0) * scaleSprite;
                }

                OnMoveAction?.Invoke(force.normalized.magnitude);
                yield return null;
            }
        }

        public void Stop()
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
            OnMoveAction?.Invoke(0);
        }

        public void Continue()
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
        }

        public void PauseMovement(bool pauseMovement)
        {
            if (pauseMovement) Stop();
            else Continue();
        }

        public void SetSpeed(float speed) => _moveSpeed = Mathf.Max(speed, 1);

        public void Move(Vector3 direction) { }
        internal void Init(float speed, float growthRate)
        {
            InvokeRepeating(nameof(CalculatePath), 0f, _repeatTimeUpdatePath);
            Continue();
        }

    }
}