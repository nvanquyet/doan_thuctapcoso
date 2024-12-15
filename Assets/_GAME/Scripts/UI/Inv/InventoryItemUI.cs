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
    [SerializeField] private TextMeshProUGUI txtName, txtDescription, txtCost;

    private Action<ItemDataSO, InventoryItemUI> OnButtonBuyClickAction;
    private ItemDataSO itemData;
    private Player target;
    private int cost;

    private void Start()
    {
        btnBuy.onClick.AddListener(OnButtonBuyClick);
    }

    public void Initialized(ItemDataSO data, int currentWave, Player target, Action<ItemDataSO, InventoryItemUI> clickBuyAction)
    {
        OnButtonBuyClickAction = clickBuyAction;
        this.target = target;
        
        this.itemData = data; 
        this.cost = (int) (data.CostTierMultiplier * GameConfig.Instance.BaseCost * Mathf.Pow(GameConfig.Instance.ScalingFactor, currentWave));

        icon.sprite = data.Appearance.Icon;
        txtName.text = data.Appearance.Name;
        txtCost.text = this.cost.ToString();
        var stringStats = "";
        foreach (var stat in data.Stat.Stats)
        {
            stringStats += $"{stat.GetStatString()}\n";
        }
        txtDescription.text = stringStats;
        btnBuy.interactable = target.CoinClaimed >= this.cost;
    }

    public void CheckInteractable()
    {
        btnBuy.interactable = target.CoinClaimed >= this.cost;
    }

    private void OnButtonBuyClick()
    {
        if (target.CoinClaimed < cost) return;
        target.CoinClaimed -= cost;
        OnButtonBuyClickAction?.Invoke(this.itemData, this);
        Destroy(gameObject);
    }
}
