using ShootingGame;
using TMPro;
using UnityEngine;
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
            if(UserData.CurrentEnergy < GameConfig.Instance.MaxEnergy)
            {
                SFX.Instance.PlaySound(AudioEvent.ButtonClick);

                //Increase energy
                UserData.CurrentEnergy++;
                Service.gI().UseEnergy(-1);
                
                //Reduce coin
                UserData.CurrentCoin -= GameConfig.Instance.PriceEnergy;
                Service.gI().AddCoin(-GameConfig.Instance.PriceEnergy);  
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
