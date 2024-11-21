using System.Collections;
using UnityEngine;

namespace ShootingGame
{
    public class BossMovement : EnemyMovement
    {
        private TypeAttack _typeBossAttack;

        public void Initialize(TypeAttack typeBossAttack)
        {
            _typeBossAttack = typeBossAttack;
        }

        protected override IEnumerator MoveToTargetCoroutine()
        {
            int currentWP = 0;
            while (currentWP < _path.vectorPath.Count)
            {

                if (_target == null) GetTarget();

                Vector2 direction = ((Vector2)_path.vectorPath[currentWP] - _rb.position).normalized;
                Vector2 force = direction * _moveSpeed * Time.deltaTime;
                transform.position += (Vector3)force;

                float distance = Vector2.Distance(_rb.position, _path.vectorPath[currentWP]);
                if (distance < _nextWayPointDistance)
                    currentWP++;

                if (force.x != 0)
                {
                    var scaleSprite = Mathf.Abs(_characterSR.transform.localScale.x);
                    if (force.x < 0)
                        _characterSR.transform.localScale = new Vector3(1, 1, 0) * scaleSprite;
                    else
                        _characterSR.transform.localScale = new Vector3(-1, 1, 0) * scaleSprite;
                }
                yield return null;
            }
        }
    }
}