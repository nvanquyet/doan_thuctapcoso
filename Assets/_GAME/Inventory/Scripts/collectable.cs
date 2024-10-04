using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class collectable : MonoBehaviour
{
    // enable pickingUp items (K)
    public Vector3 posToGo;
    public TetrisItem itemTetris;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            if (Input.GetKey(KeyCode.K)) //take the item
            {
                bool wasPickedUpTestris = false;
                wasPickedUpTestris = TetrisSlot.Instance.AddInFirstSpace(itemTetris); //add to the bag matrix.
                if (wasPickedUpTestris) // took
                {                    
                    Destroy(this.gameObject);
                }
            }
        }
    }

}
