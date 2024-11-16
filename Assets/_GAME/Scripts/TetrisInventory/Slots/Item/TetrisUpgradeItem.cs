using System;
using ShootingGame;
using UnityEngine;

public class TetrisUpgradeItem : AStayInteractor<BoxCollider2D>
{
    private TetrisItemSlot slot;
    private Action<TetrisItemSlot> OnUpgradeItem;


    public void Init(TetrisItemSlot slot, Action<TetrisItemSlot> onUpgradeItem)
    {
        this.slot = slot;
        this.OnUpgradeItem = onUpgradeItem;

        RescaleCollider();
    }


    public bool CheckInteract()
    {
       
        foreach(var i in _interactables){
            if(i is TetrisUpgradeItem item){
                if (slot.itemData.NextLevelAttribute == null) continue;
                if (item.slot.itemData == slot.itemData)
                {
                    item.slot.OnDestroyItem();
                    slot.OnDestroyItem();

                    OnUpgradeItem?.Invoke(item.slot);

                    Destroy(item.gameObject);
                    Destroy(gameObject);
                    return true;
                }
            }
            if(i is TetrisRemoveItem removeItem){
                GameService.LogColor("Remove Item");
                removeItem.InvokeAction(slot);
                return true;
            }
        }
        return false;
    }


    private void RescaleCollider(){
        //Rescale Collider
        var sizeUI = slot.RectTransform.sizeDelta;
        if(Collider is BoxCollider2D boxCollider2D){
            boxCollider2D.size = sizeUI;
            //boxCollider2D.offset = new Vector2(sizeUI.x / 2, -sizeUI.y / 2);
        }
    }

    public override void OnInteract(Interface.IInteract target)
    {
        
    }
    public override void ExitInteract(Interface.IInteract target)
    {
        
    }
}
