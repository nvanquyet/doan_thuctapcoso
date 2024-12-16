using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : Frame
{
    [SerializeField] private TextMeshProUGUI userTxt;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSetting;
    private void Start()
    {
        userTxt?.SetText(UserData.UserName);
        btnPlay?.onClick.AddListener(() =>
        {
            UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync(2);
        });
        btnSetting?.onClick.AddListener(() =>
        {
            UIPopUpCtrl.Instance.Get<UISetting>().Show();
        });
    }
}
