using System;
using ShootingGame;
using ShootingGame.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TetrisInventory : Frame
{
    [SerializeField] private int numberSlots = 64;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private TetrisUI tetrisUI;
    [SerializeField] private TetrisSlot tetrisSlot;
    [SerializeField] private WaitingSlots waitingSlots;
    [SerializeField] private TetrisItemDescription tetrisDescription;
    [SerializeField] private TetrisRemoveItem tetrisRemoveItem;
    [SerializeField] private ItemAttributeData[] itemTest;

    [SerializeField] private Button btnPlay, btnBack;
    public Action OnBtnPlayClick, OnBtnBackClick;

#if UNITY_EDITOR
    private void OnValidate()
    {
        tetrisUI = GetComponentInChildren<TetrisUI>();
        tetrisUI.SetCellSize(cellSize);
        tetrisSlot = GetComponentInChildren<TetrisSlot>();
        tetrisSlot.SetCellSize(cellSize);
        waitingSlots = GetComponentInChildren<WaitingSlots>();
        tetrisDescription = GetComponentInChildren<TetrisItemDescription>();
        tetrisRemoveItem = GetComponentInChildren<TetrisRemoveItem>();
    }
#endif

    private void Start(){
        SetNumberSlots(this.numberSlots);
        tetrisRemoveItem.SetAction(OnRemoveItem);
        btnPlay.onClick.AddListener(() => OnBtnPlayClick?.Invoke());
        btnBack.onClick.AddListener(() => OnBtnBackClick?.Invoke());
    }

    public void SetNumberSlots(int numberSlots)
    {
        this.numberSlots = numberSlots;
        tetrisSlot?.SetGrid(numberSlots);
        tetrisUI?.CreateSlot(numberSlots);
    }

    public void OnAddToTetrisSlot(TetrisItemSlot item)
    {
        waitingSlots.RemoveItem(item);
        tetrisSlot.AddItem(item);
    }

    public void OnRemoveItem(TetrisItemSlot item)
    {
        tetrisSlot.RemoveItem(item);
        waitingSlots.RemoveItem(item);
    }

    public void OnReturnToWaitingList(TetrisItemSlot item)
    {
        waitingSlots.AddItem(item);
        tetrisSlot.RemoveItem(item);
    }
    public void AddItem(ItemAttributeData[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            CreateNewItem(items[i]);
        }
    }


    public TetrisItemSlot CreateNewItem(ItemAttributeData item)
    {
        var itemSlot = tetrisSlot.CreateItem(tetrisDescription, item);
        OnReturnToWaitingList(itemSlot);
        itemSlot.InitAction(OnReturnToWaitingList, OnAddToTetrisSlot, 
            tetrisUI.OnMarkItemInGrid, OnUpgradeItem, OnRemoveItem);
        return itemSlot;
    }

    private void OnUpgradeItem(TetrisItemSlot slot)
    {
        if (slot.itemData.NextLevelAttribute == null) return;
        var newItem = CreateNewItem(slot.itemData.NextLevelAttribute);
        newItem.transform.position = slot.transform.position;
        newItem.transform.SetParent(slot.transform.parent);
        if (newItem.transform.parent.Equals(tetrisSlot.transform))
        {
            OnAddToTetrisSlot(newItem);
            newItem.OnEndDrag();
        }
        else OnReturnToWaitingList(newItem);
    }

    public int[] GetTetrisItemsID(){
        int[] allItems = new int[tetrisSlot.ItemsInBag.Count];
        for(int i = 0; i < tetrisSlot.ItemsInBag.Count; i++)
        {
            allItems[i] = Array.IndexOf(GameData.Instance.ItemData.GetAllValue(), tetrisSlot.ItemsInBag[i].itemData);
        }
        return allItems;
    }

}
