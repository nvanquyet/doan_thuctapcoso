
using System.Collections.Generic;
using System.Linq;
using ShootingGame;
using ShootingGame.Data;
using UnityEngine;
public class InventorySystem : Frame
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

    private void OnItemClicked(ItemDataSO data)
    {
        if (data == null) return;
        if(claimedItems == null) claimedItems = new List<ItemDataSO>();
        claimedItems.Add(data);
    }

    private void OnButtonPlayGameClick()
    {
        //Call next wave
        UICtrl.Instance.Hide<InventorySystem>(true, () => {
            GameCtrl.Instance.NextWave();
            if(tetrisInventory.GetTetrisItem(out var weapons, out var equiqment, out var buffItems))
            {
                this.Dispatch<GameEvent.OnNextWave>(new GameEvent.OnNextWave { allWeapons = weapons, allEquiqments = equiqment , allBuffItems = buffItems});
            }
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
