using Google.MiniJSON;
using ShootingGame;
using ShootingGame.Data;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemEnergyShop : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button btnBuy;

    private void Start()
    {
        this.AddListener<GameEvent.CoinChange>(OnCoinChange, false);
        this.AddListener<GameEvent.EnergyChange>(OnEnergyChange, false);
        btnBuy.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            if(UserData.CurrentEnergy < GameConfig.Instance.MaxEnergy)
            {
                UserData.CurrentCoin -= GameConfig.Instance.PriceEnergy;
                UserData.CurrentEnergy++;
            }
        });

        priceText.text = GameConfig.Instance.PriceEnergy.ToString();

    }

    private void OnEnable()
    {
        OnCheckBtnBuy();
    }

    private void OnCoinChange(GameEvent.CoinChange param) => OnCheckBtnBuy();
    private void OnEnergyChange(GameEvent.EnergyChange param) => OnCheckBtnBuy();

    private void OnCheckBtnBuy()
    {
        if (UserData.CurrentCoin < GameConfig.Instance.PriceEnergy || UserData.CurrentEnergy >= GameConfig.Instance.MaxEnergy) btnBuy.interactable = false;
        else btnBuy.interactable = true;
    }
}
