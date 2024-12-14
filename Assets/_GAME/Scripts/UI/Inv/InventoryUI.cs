using ShootingGame;
using ShootingGame.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Frame
{
    [SerializeField] private Button btnContinue;

    [SerializeField] private Button btnRandomItem;
    [SerializeField] private TextMeshProUGUI textRandomItem;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private InventoryItemUI inventoryItemUI;
    [SerializeField] private int maxItem = 5;

    public Action<ItemDataSO> OnItemClickedAction;

    private int coinPurchase = 0;
    private int currentWave = 0;

    private Player player;
    public Action OnContinueAction;

    public void SetTarget(Player player) => this.player = player;
    public void Initialized(int wave, bool levelUp = false)
    {
        if (levelUp)
        {
            OnButtonRandomItemClick();
            this.currentWave = wave;
            coinPurchase = (int)(GameConfig.Instance.BaseCost / 2 * Mathf.Pow(GameConfig.Instance.ScalingFactor, wave));
            textRandomItem.text = coinPurchase.ToString();
            btnRandomItem.interactable = player.CoinClaimed >= coinPurchase;
            UICtrl.Instance.Show<InventoryUI>();
        }
    }

    private void Start()
    {
        btnContinue.onClick.RemoveAllListeners();
        btnContinue.onClick.AddListener(OnContinueButtonClicked);

        btnRandomItem.onClick.RemoveAllListeners();
        btnRandomItem.onClick.AddListener(OnButtonRandomItemClick);
    }

    public void OnButtonRandomItemClick()
    {
        btnRandomItem.interactable = false;
        var randomAllItems = RandomItems(maxItem);

        //Remove all Item in Placeholder
        if (placeHolder)
        {
            foreach (Transform child in placeHolder)
            {
                Destroy(child.gameObject);
            }
        }

        //Random Item Here
        var allItems = GameData.Instance.ItemData.GetAllValue();
        for (int i = 0; i < maxItem; i++)
        {
            var item = Instantiate(inventoryItemUI, placeHolder);
            if(i >= randomAllItems.Count)
            {
                i = i % randomAllItems.Count;
            }
            if(randomAllItems[i] == null) continue;
            item.Initialized(randomAllItems[i], this.currentWave, (item, cost) =>
            {
                if (player.CoinClaimed < cost) return;
                player.CoinClaimed -= cost;
                OnItemClickedAction?.Invoke(item);
            });
            item.gameObject.SetActive(true);
        }

        //Decrease Coin
        player.CoinClaimed -= coinPurchase;
        Invoke(nameof(ActiveButtonRandom), 0.12f);    
    }
    private void ActiveButtonRandom()
    {
        btnRandomItem.interactable = player.CoinClaimed >= coinPurchase;
    }
    private List<ItemDataSO> RandomItems(int capacity)
    {
        capacity = Mathf.Max(capacity, 5);
        var weapons = (GameData.Instance.ItemData.GetValue(Category.Weapon) as WeaponData).GetAllValue().ToList();
        var equiqments = (GameData.Instance.ItemData.GetValue(Category.Equiqment) as EquiqmentData).GetAllValue().ToList();
        var buffItems = (GameData.Instance.ItemData.GetValue(Category.BuffItem) as BuffItemData).GetAllValue().ToList();
        List<ItemDataSO> result = new List<ItemDataSO>();
        //Random amount of weapon, equipment, buff
        int weaponCount = UnityEngine.Random.Range(1, 3);
        int equipmentCount = UnityEngine.Random.Range(2, 4);
        int buffCount = capacity - weaponCount - equipmentCount;
        GameService.LogColor($"RandomItems {weaponCount} {equipmentCount} {buffCount}");

        // Random weapon
        if(weaponCount > 0)
        {
            GameService.LogColor($"RandomItems Weapons");
            for (int i = 0; i < weaponCount; i++)
            {
                var weapon = GameService.RandomItem(weapons);
                if (weapon != null) result.Add(weapon);
            }
        } 
        
        if(equipmentCount > 0)
        {
            GameService.LogColor($"RandomItems Equiqment");
            // Random equiqment
            for (int i = 0; i < equipmentCount; i++)
            {
                var equipment = GameService.RandomItem(equiqments);
                if (equipment != null) result.Add(equipment);
            }
        }
        if(buffCount > 0)
        {
            GameService.LogColor($"RandomItems Buff");
            // Random buff
            for (int i = 0; i < buffCount; i++)
            {
                var buff = GameService.RandomItem(buffItems);
                if (buff != null) result.Add(buff);
            }
        }
      

        

        return result;
    }

    private void OnContinueButtonClicked()
    {
        OnContinueAction?.Invoke();
    }
}
