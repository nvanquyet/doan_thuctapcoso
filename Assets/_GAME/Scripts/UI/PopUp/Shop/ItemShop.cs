using ShootingGame;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameItem;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI ownText;
    [SerializeField] private GameObject ownObject;
    [SerializeField] private Button btnBuy;

    private int price;
    private bool isOwn;

    private void Start()
    {
        this.AddListener<GameEvent.CoinChange>(OnCoinChange, false);
    }

    private void OnCoinChange(GameEvent.CoinChange param)
    {
        if (!isOwn && UserData.CurrentCoin < price) btnBuy.interactable = false; 
        else btnBuy.interactable = true;
    }

    public void InitData(Sprite icon, string name, string price, bool isOwn, Action onBuy = null)
    {
        this.icon.sprite = icon;
        this.nameItem.text = name;
        this.priceText.text = price;
        btnBuy.onClick.AddListener(() => onBuy?.Invoke());
        btnBuy.gameObject.SetActive(!isOwn);
        this.ownObject.SetActive(isOwn);
        if(isOwn) ownText?.SetText("Own");
    }

}
