
using System.Collections.Generic;
using System.Linq;
using ShootingGame;
using ShootingGame.Data;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private TetrisInventory tetrisInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private List<ItemDataSO> claimedItems;
    [SerializeField] private Player player;
#if UNITY_EDITOR
    private void OnValidate()
    {
        tetrisInventory = GetComponentInChildren<TetrisInventory>();
        inventoryUI = GetComponentInChildren<InventoryUI>();
        player = FindObjectOfType<Player>();
    }
    #endif

    private void Start()
    {
        this.AddListener<GameEvent.OnWaveClear>(OnWaveClear, false);

        claimedItems = new List<ItemDataSO>();
        inventoryUI.OnItemClickedAction += OnItemClicked;
        tetrisInventory.OnBackAction += () =>
        {
            UICtrl.Instance.Show<InventoryUI>();
        };
        inventoryUI.OnContinueAction += () =>
        {
            OnShowTetrisUI();
            UICtrl.Instance.Show<TetrisInventory>();
        };

        inventoryUI.gameObject.SetActive(false);
        tetrisInventory.gameObject.SetActive(false);

        var startBuyItem = GameConfig.Instance.startGameBuyItem;
        if(!startBuyItem)
        {
            var weaponRandom = GameData.Instance.ItemData.GetValue(Category.Weapon) as WeaponData;
            var item = GameService.RandomItem(weaponRandom.GetAllValue().ToList());

            OnItemClicked(item);
            OnShowTetrisUI();
        }
        tetrisInventory.Initialized(startBuyItem); 
        inventoryUI.SetTarget(player);
        inventoryUI.Initialized(0, startBuyItem);
    }

    private void OnItemClicked(ItemDataSO data)
    {
        if (data == null) return;
        if(claimedItems == null) claimedItems = new List<ItemDataSO>();
        claimedItems.Add(data);
    }


    private void OnWaveClear(GameEvent.OnWaveClear param)
    {
        inventoryUI.Initialized(param.wave, player.IsLevelUp);
        tetrisInventory.Initialized(player.IsLevelUp);
    }

    private void OnShowTetrisUI()
    {
        tetrisInventory.AddItem(claimedItems.ToArray());
        claimedItems.Clear();
    }
}
