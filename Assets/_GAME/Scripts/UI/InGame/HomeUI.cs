using ShootingGame;
using TMPro;
using UnityEngine;

public class HomeUI : Frame
{ 
    [SerializeField] private TextMeshProUGUI userTxt;
    private void Start()
    {
        userTxt?.SetText(UserData.UserName);
    }
}
