using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Server
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NetworkPlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float accelerationTime = 0.1f;
        [SerializeField] private SpriteRenderer characterSR;

        private Rigidbody2D rb;

        private Vector2 movementInput;
        private Vector2 currentVelocity;

        private NetworkVariable<Vector2> networkedPosition = new NetworkVariable<Vector2>();

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
            if (IsServer)
            {
                networkedPosition.Value = Rigid.position;
            }
        }

        void Update()
        {
            if (IsLocalPlayer)
            {
                movementInput.x = Input.GetAxisRaw("Horizontal");
                movementInput.y = Input.GetAxisRaw("Vertical");
                SendMovementInputServerRpc(movementInput);


                //Sent to Server 
                if (movementInput.x != 0)
                {
                    Vector3 newScale = characterSR.transform.localScale;
                    if (movementInput.x < 0)
                        newScale.x = -1 * Mathf.Abs(newScale.x);
                    else
                        newScale.x = Mathf.Abs(newScale.x);

                    SendScaleChangeServerRpc(newScale);
                }
            }
            else
            {
                // Update the client's position from the server
                Rigid.position = networkedPosition.Value;
            }
        }
       

        void FixedUpdate()
        {
            if (IsServer)
            {
                Movement();
            }
        }

        private void Movement()
        { 
            Vector2 targetVelocity = movementInput.normalized * moveSpeed;
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accelerationTime / Time.fixedDeltaTime);
            Vector2 newPosition = Rigid.position + currentVelocity * Time.fixedDeltaTime;
            Rigid.MovePosition(newPosition);

            // Sync position with all clients
            networkedPosition.Value = newPosition;
        }

        [ServerRpc]
        private void SendMovementInputServerRpc(Vector2 movementInput, ServerRpcParams rpcParams = default)
        {
            this.movementInput = movementInput;
        }

        [ServerRpc]
        private void SendScaleChangeServerRpc(Vector3 newScale)
        {
            characterSR.transform.localScale = newScale;
            UpdateScaleClientRpc(newScale);
        }
        [ClientRpc]
        private void UpdateScaleClientRpc(Vector3 newScale)
        {
            characterSR.transform.localScale = newScale;
        }
    }
}
