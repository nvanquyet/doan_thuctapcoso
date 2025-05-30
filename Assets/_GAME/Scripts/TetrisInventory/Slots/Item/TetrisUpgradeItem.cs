using System;
using ShootingGame;
using ShootingGame.Data;
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
                if(slot.itemData is ItemEquiqmentData)
                {
                    var it = slot.itemData as ItemEquiqmentData;
                    if (it.NextLevelAttribute == null) continue;
                    if(item.slot != null)
                    {
                        if (item.slot.itemData == slot.itemData && Vector3.Distance(item.slot.transform.position, slot.transform.position) < 32f)
                        {
                            if (item.slot) item.slot.OnDestroyItem();
                            if (slot) slot.OnDestroyItem();

                            OnUpgradeItem?.Invoke(item.slot);

                            Destroy(item.gameObject); 
                            Destroy(gameObject);

                            return true;
                        }
                    }
                }
                continue;
            }
            if(i is TetrisRemoveItem removeItem){
                var distance = Vector2.Distance(transform.position, removeItem.transform.position);
                if(distance < 24)
                {
                    removeItem.InvokeAction(slot);
                    return true;
                }
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
