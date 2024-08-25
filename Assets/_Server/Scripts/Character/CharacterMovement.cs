using Mirror;
using System;
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

        [Client]

        private void Start()
        {
            if (!isLocalPlayer) return;
            if (Rigid != null) Rigid.simulated = true;
        }

        [Client]
        void Update()
        {
            if (!isLocalPlayer) return;
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
        }
        [Client]
        void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            CmdMove();
        }

        [Command]
        private void CmdMove()
        {
            // Sync the movement across all clients
            RpcMove();
        }

        [ClientRpc]
        private void RpcMove()
        {
            // Update position on all clients
            Vector2 targetVelocity = movementInput.normalized * moveSpeed;
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accelerationTime / Time.fixedDeltaTime);
            Rigid.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
        }
    }

}