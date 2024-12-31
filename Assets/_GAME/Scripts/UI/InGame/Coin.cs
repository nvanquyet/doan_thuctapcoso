using ShootingGame;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        this.AddListener<GameEvent.CoinChange>(OnCoinChange, false);
    }

    private void OnEnable()
    {
        coinText?.SetText(UserData.CurrentCoin.ToString());
    }

    private void OnCoinChange(GameEvent.CoinChange param)
    {
        coinText?.SetText(UserData.CurrentCoin.ToString());
    }
}
 