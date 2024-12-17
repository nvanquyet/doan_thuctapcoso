using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : Frame
{
    [SerializeField] private TextMeshProUGUI userTxt;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnHelp;
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnInventory;
    private void Start()
    {
        userTxt?.SetText(UserData.UserName);
        btnPlay?.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync((int)SceneIndex.InGame);
        });
        btnShop?.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UIPopUpCtrl.Instance.Show<UIInventory>();
        });
        btnHelp?.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UIPopUpCtrl.Instance.Show<UIGuide>();
        });
        btnInventory?.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UIPopUpCtrl.Instance.Show<UIInventory>();
        });
        btnSetting?.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UIPopUpCtrl.Instance.Show<UISetting>();
        });
    }
}
