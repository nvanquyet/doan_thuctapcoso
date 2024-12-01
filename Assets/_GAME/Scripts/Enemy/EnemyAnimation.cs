using System;
using UnityEngine;
namespace ShootingGame
{

    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private const string VELOCITY = "Velocity";
        private const string ATTACK = "Attack";
        private const string DEAD = "Dead";
        private const string RESPAWN = "Respawn";
        private const string DEFEND = "Hit";

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

#endif


        public void SetVelocity(float velocity) => _animator.SetFloat(VELOCITY, velocity);

        public void OnTriggerDead() => _animator?.SetTrigger(DEAD);

        public void OnTriggerDefend() => _animator?.SetTrigger(DEFEND);

        public void OnTriggerRespawn() => _animator?.SetTrigger(RESPAWN);
    }
}