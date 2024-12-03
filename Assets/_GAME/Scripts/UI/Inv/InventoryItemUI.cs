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
    [SerializeField] private TextMeshProUGUI txtName, txtDescription;

    private Action<ItemDataSO> OnButtonBuyClickAction;
    private ItemDataSO itemData;

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
    }


    private void OnButtonBuyClick()
    {
        OnButtonBuyClickAction?.Invoke(itemData);
        Destroy(gameObject);
    }
}
