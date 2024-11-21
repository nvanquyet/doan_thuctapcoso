using ShootingGame;
using ShootingGame.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : UIComponent
{
    [SerializeField] private Button btnBuy;
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
    }


    private void OnButtonBuyClick() => OnButtonBuyClickAction?.Invoke(itemAttributeData);
}
