using DG.Tweening;
using ShootingGame;
using ShootingGame.Data;
using System;
using System.Collections;
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
    [SerializeField] private LayoutGroup layoutGroup;
    [SerializeField] private int maxItem = 5;

    public Action<ItemDataSO> OnItemClickedAction;

    private int coinPurchase = 0;
    private int currentWave = 0;

    private Player player;
    public Action OnContinueAction;
    private List<InventoryItemUI> inventoryItemUIs = new List<InventoryItemUI>();
    public void SetTarget(Player player) => this.player = player;
    public void Initialized(int wave, bool levelUp = false)
    {
        if (levelUp)
        {
            inventoryItemUIs = new List<InventoryItemUI>();
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
        btnRandomItem.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            OnButtonRandomItemClick();
        });
    }

    public void OnButtonRandomItemClick()
    {
        if (btnRandomItem) btnRandomItem.interactable = false;

        if (layoutGroup) layoutGroup.enabled = true;

        DOVirtual.DelayedCall(0.15f, () =>
        {
            StartCoroutine(IERandomItem(() =>
            {
                if (layoutGroup) layoutGroup.enabled = false;
                btnRandomItem.interactable = player.CoinClaimed >= coinPurchase;
            }));
        });
    }


    private IEnumerator IERandomItem(Action callback = null)
    {

        var randomAllItems = RandomItems(maxItem);
        if (randomAllItems == null || randomAllItems.Count <= 0)
        {
            callback?.Invoke();
            yield break;
        }

        var allItems = GameData.Instance.ItemData.GetAllValue();
        if (allItems == null || allItems.Length <= 0)
        {
            callback?.Invoke();
            yield break;
        }

        //Remove all Item in Placeholder
        if (placeHolder)
        {
            foreach (Transform child in placeHolder)
            {
                if (child) Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < maxItem; i++)
        {
            var item = Instantiate(inventoryItemUI, placeHolder);
            if (item == null) continue;

            if (i >= randomAllItems.Count)
            {
                i = i % randomAllItems.Count;
            }
            if (randomAllItems[i] == null) continue;

            item.Initialized(randomAllItems[i], this.currentWave, player, (data, item) =>
            {
                if(inventoryItemUIs.Contains(item)) inventoryItemUIs.Remove(item);
                foreach (var i in inventoryItemUIs)
                {
                    if(i != null) i.CheckInteractable();
                }
                btnRandomItem.interactable = player.CoinClaimed >= coinPurchase;
                OnItemClickedAction?.Invoke(data);
            });

            item.gameObject.SetActive(true);
            inventoryItemUIs.Add(item);
            yield return null;
        }

        //Decrease Coin
        player.CoinClaimed -= coinPurchase;
        yield return new WaitForSeconds(0.1f);
        //Make sure only 5 item
        if (placeHolder)
        {
            for(int i = placeHolder.childCount - 1; i >= maxItem; i--)
            {
                Destroy(placeHolder.GetChild(i).gameObject);
            }
        }
        callback?.Invoke();
    }

    private List<ItemDataSO> RandomItems(int capacity)
    {
        var itemData = GameData.Instance.ItemData;
        if (itemData == null) return null;

        capacity = Mathf.Max(capacity, 5);
        //Random amount of weapon, equipment, buff
        int weaponCount = UnityEngine.Random.Range(1, 3);
        int equipmentCount = UnityEngine.Random.Range(2, 4);
        int buffCount = capacity - weaponCount - equipmentCount;

        List<ItemWeaponData> weapons = new List<ItemWeaponData>();
        List<ItemEquiqmentData> equiqments = new List<ItemEquiqmentData>();
        List<ItemBuffData> buffItems = new List<ItemBuffData>();

        var w = itemData.GetValue(Category.Weapon);
        if (w is WeaponData) weapons = (w as WeaponData).GetAllValue().ToList();
        else return null;

        var b = itemData.GetValue(Category.BuffItem);
        if (b is BuffItemData) buffItems = (b as BuffItemData).GetAllValue().ToList();
        else return null;

        var e = itemData.GetValue(Category.Equiqment);
        if (e is EquiqmentData) equiqments = (e as EquiqmentData).GetAllValue().ToList();
        else return null;

        List<ItemDataSO> result = new List<ItemDataSO>();

        // Random weapon
        if (weaponCount > 0)
        {
            for (int i = 0; i < weaponCount; i++)
            {
                var weapon = GameService.RandomItem(weapons);
                if (weapon != null) result.Add(weapon);
            }
        }

        if (equipmentCount > 0)
        {
            // Random equiqment
            for (int i = 0; i < equipmentCount; i++)
            {
                var equipment = GameService.RandomItem(equiqments);
                if (equipment != null) result.Add(equipment);
            }
        }
        if (buffCount > 0)
        {
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
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        OnContinueAction?.Invoke();
    }
}
