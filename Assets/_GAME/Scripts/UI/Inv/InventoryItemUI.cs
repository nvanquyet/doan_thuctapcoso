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

    private Action<ItemDataSO, int> OnButtonBuyClickAction;
    private ItemDataSO itemData;

    private int cost;

    private void Start()
    {
        btnBuy.onClick.AddListener(OnButtonBuyClick);
    }

    public void Initialized(ItemDataSO data, int currentWave, Action<ItemDataSO, int> clickBuyAction)
    {
        OnButtonBuyClickAction = clickBuyAction;
        
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
    }


    private void OnButtonBuyClick()
    {
        OnButtonBuyClickAction?.Invoke(this.itemData, this.cost);
        Destroy(gameObject);
    }
}
