
using System.Collections.Generic;
using System.Linq;
using ShootingGame;
using ShootingGame.Data;
using UnityEngine;
public class InventorySystem : Frame
{
    [SerializeField] private TetrisInventory tetrisInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private List<ItemAttributeData> claimedItems;
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
        claimedItems = new List<ItemAttributeData>();
        inventoryUI.OnItemClickedAction += OnItemClicked;
        inventoryUI.OnButtonTetrisInvClickAction += () => {
            OnShowTetrisUI();
            inventoryUI.Hide();
            tetrisInventory.Show();
        };

        tetrisInventory.OnBtnPlayClick += OnButtonPlayGameClick;
        tetrisInventory.OnBtnBackClick += () => {
            inventoryUI.Show();
            tetrisInventory.Hide();
        };

        inventoryUI.gameObject.SetActive(false);
        inventoryUI.Show(true, () =>
        {
            inventoryUI.OnButtonRandomItemClick();
        });
        tetrisInventory.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        inventoryUI.Show(true, () =>
        {
            inventoryUI.OnButtonRandomItemClick();
        });
        tetrisInventory.gameObject.SetActive(false);
    }

    private void OnItemClicked(ItemAttributeData data)
    {
        if (data == null) return;
        if(claimedItems == null) claimedItems = new List<ItemAttributeData>();
        claimedItems.Add(data);
        GameService.LogColor("Claimed Item: " + data.Appearance.Name);
    }

    private void OnButtonPlayGameClick()
    {
        //Call next wave
        UICtrl.Instance.Hide<InventorySystem>(true, () => {
            GameCtrl.Instance.NextWave();
            var allItemsID = tetrisInventory.GetTetrisItemsID();
            string allId = string.Join(",", allItemsID);
            GameService.LogColor("All ID: " + allId);
            this.Dispatch<GameEvent.OnNextWave>(new GameEvent.OnNextWave { allIDItem = allItemsID.ToList() });
        });
    }

    private void OnWaveClear()
    {
        UICtrl.Instance.Show<InventorySystem>();
    }

    private void OnShowTetrisUI()
    {
        tetrisInventory.AddItem(claimedItems.ToArray());
        claimedItems.Clear();
    }
}
