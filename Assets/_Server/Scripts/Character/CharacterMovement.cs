
using System;
using Unity.Netcode;
using UnityEngine;
namespace Server
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float accelerationTime = 0.1f;
        
        private Rigidbody2D rb;

        private Vector2 movementInput;
        private Vector2 currentVelocity;


        private Rigidbody2D Rigid
        {
            get
            {
                if (rb == null) rb = GetComponent<Rigidbody2D>();
                return rb;
            }
        }


        private void Start()
        {
            if (!IsLocalPlayer) return;
            if (Rigid != null) Rigid.simulated = true;
        }

        void Update()
        {
            if (!IsLocalPlayer) return;
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            if (!IsLocalPlayer) return;
            Movement();
        }


        private void Movement()
        {
            // Update position on all clients
            Vector2 targetVelocity = movementInput.normalized * moveSpeed;
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accelerationTime / Time.fixedDeltaTime);
            Rigid.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
        }
    }

}