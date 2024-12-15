using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private void OnEnable()
    {
        coinText?.SetText(UserData.CurrentCoin.ToString());
    }
}
 