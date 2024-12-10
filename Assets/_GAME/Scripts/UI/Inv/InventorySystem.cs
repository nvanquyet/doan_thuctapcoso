
using System.Collections.Generic;
using ShootingGame;
using ShootingGame.Data;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private TetrisInventory tetrisInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private List<ItemDataSO> claimedItems;
#if UNITY_EDITOR
    private void OnValidate()
    {
        tetrisInventory = GetComponentInChildren<TetrisInventory>();
        inventoryUI = GetComponentInChildren<InventoryUI>();
    }
    #endif

    private void Start()
    {
        this.AddListener<GameEvent.OnWaveClear>(OnWaveClear, false);

        claimedItems = new List<ItemDataSO>();
        inventoryUI.OnItemClickedAction += OnItemClicked;

        inventoryUI.gameObject.SetActive(false);
        tetrisInventory.gameObject.SetActive(false);

        inventoryUI.Initialized(OnWaveClear);
    }

    private void OnItemClicked(ItemDataSO data)
    {
        if (data == null) return;
        if(claimedItems == null) claimedItems = new List<ItemDataSO>();
        claimedItems.Add(data);
    }


    private void OnWaveClear()
    {
        OnShowTetrisUI();
        UICtrl.Instance.Show<TetrisInventory>();
    }

    private void OnShowTetrisUI()
    {
        tetrisInventory.AddItem(claimedItems.ToArray());
        claimedItems.Clear();
    }
}
