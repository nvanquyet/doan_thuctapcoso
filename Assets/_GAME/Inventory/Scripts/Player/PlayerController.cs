using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //dumb movement script
    public float moveSpeedNormal = 6.5f;
    public Rigidbody2D rb;
    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // -1 a 1          
        movement.y = Input.GetAxisRaw("Vertical"); // -1 a 1   
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeedNormal * Time.fixedDeltaTime);
    }
}
