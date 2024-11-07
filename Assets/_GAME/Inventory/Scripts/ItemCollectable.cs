using ShootingGame;
using UnityEngine;

public class ItemCollectable : AInteractable<BoxCollider2D>
{
    public TetrisItem itemTetris;
    public override void ExitInteract(Interface.IInteract target)
    {
        
    }

    public override void OnInteract(Interface.IInteract target)
    {
        
    }

    // public override void OnInteract(Interface.IInteract target)
    // {
    //     if(target is ItemCollector){
    //         if (Input.GetKey(KeyCode.K)) 
    //         {
    //             bool wasPickedUpTestris = false;
    //             wasPickedUpTestris = TetrisSlot.Instance.AddInFirstSpace(itemTetris); 
    //             if (wasPickedUpTestris) // took
    //             {                    
    //                 Destroy(this.gameObject);
    //             }
    //         }
    //     }
    // }
}
