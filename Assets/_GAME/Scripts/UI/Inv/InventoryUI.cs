using ShootingGame;
using ShootingGame.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Frame
{
    [SerializeField] private Button btnContinue;
    [SerializeField] private Button btnRandomItem;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private InventoryItemUI inventoryItemUI;
    [SerializeField] private int maxItem = 5;

    public Action<ItemDataSO> OnItemClickedAction;

    public Action OnContinueAction;

    public void Initialized(bool levelUp = false)
    {
        if (levelUp)
        {
            OnButtonRandomItemClick();
            UICtrl.Instance.Show<InventoryUI>();
        }
    }

    private void Start()
    {
        btnContinue.onClick.AddListener(OnContinueButtonClicked);
        btnRandomItem.onClick.AddListener(OnButtonRandomItemClick);
    }

    public void OnButtonRandomItemClick()
    {
        var randomAllItems = RandomItems(maxItem);

        //Remove all Item in Placeholder
        foreach (Transform child in placeHolder)
        {
            Destroy(child.gameObject);
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
            item.Initialized(randomAllItems[i], OnItemClickedAction);
            item.gameObject.SetActive(true);
        }
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

        // Random weapon
        for (int i = 0; i < weaponCount; i++)
        {
            var weapon = GameService.RandomItem(weapons);
            if (weapon != null) result.Add(weapon);
        }

        // Random equiqment
        for (int i = 0; i < equipmentCount; i++)
        {
            var equipment = GameService.RandomItem(equiqments);
            if (equipment != null) result.Add(equipment);
        }

        // Random buff
        for (int i = 0; i < buffCount; i++)
        {
            var buff = GameService.RandomItem(buffItems);
            if (buff != null) result.Add(buff);
        }

        return result;
    }

    private void OnContinueButtonClicked()
    {
        OnContinueAction?.Invoke();
    }
}
