using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    //dumb camera follow player script
    public Transform target; // player
    public Vector3 offset;

    private void FixedUpdate()
    {
            transform.position = (target.position + offset);
    }
   
}
