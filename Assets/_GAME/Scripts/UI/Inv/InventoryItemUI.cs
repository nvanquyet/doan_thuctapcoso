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

    private Action<ItemAttributeData> OnButtonBuyClickAction;
    private ItemAttributeData itemAttributeData;

    private void Start()
    {
        btnBuy.onClick.AddListener(OnButtonBuyClick);
    }

    public void Initialized(ItemAttributeData data, Action<ItemAttributeData> clickBuyAction)
    {
        OnButtonBuyClickAction = clickBuyAction;
        itemAttributeData = data;
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
        OnButtonBuyClickAction?.Invoke(itemAttributeData);
        Destroy(gameObject);
    }
}
