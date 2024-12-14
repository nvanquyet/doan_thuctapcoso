using ShootingGame;
using ShootingGame.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : UIComponent
{
    [SerializeField] private Button btnBuy;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtName, txtDescription, textCoin, currentCoin;
    



    private Action<ItemDataSO> OnButtonBuyClickAction;
    private ItemDataSO itemData;

    private CoinManager coinManager => CoinManager.Instance;

    private void Start()
    {
        btnBuy.onClick.AddListener(OnButtonBuyClick);
    }

    public void Initialized(ItemDataSO data, Action<ItemDataSO> clickBuyAction)
    {
        OnButtonBuyClickAction = clickBuyAction;
        itemData = data;

        icon.sprite = data.Appearance.Icon;
        txtName.text = data.Appearance.Name;

        var stringStats = "";
        foreach (var stat in data.Stat.Stats)
        {
            stringStats += $"{stat.GetStatString()}\n";
        }
        txtDescription.text = stringStats;

        textCoin.text = itemData.BaseCoin.ToString();
        
    }

    
    public bool TryPurchaseItem(int itemCost)
    {
        int currentCoins = coinManager.GetCoin(); 

        if (itemCost <= currentCoins) 
        {
            coinManager.RemoveCoin(itemCost); 
            Debug.Log($"Item purchased! Remaining coins: {coinManager.GetCoin()}");
            return true;
        }
        else
        {
            Debug.Log("Not enough coins to purchase this item.");
            return false;
        }
    }

    
    private void OnButtonBuyClick()
    {
        if (itemData == null) return;

        int itemCost = itemData.BaseCoin;

        if (TryPurchaseItem(itemCost))
        {        
            Debug.Log($"Purchased item: {itemData.Appearance.Name}");
            OnButtonBuyClickAction?.Invoke(itemData); 
            Destroy(gameObject); 
        }
        else
        {
            Debug.LogWarning("You cannot afford this item.");
        }
    }
    private void Update()
    {
        currentCoin.text = coinManager.GetCoin().ToString();
    }
}
