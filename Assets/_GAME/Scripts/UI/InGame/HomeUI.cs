using ShootingGame;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEvent;

public class HomeUI : Frame
{
    [SerializeField] private TextMeshProUGUI userTxt;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnHelp;
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnInventory;

    [SerializeField] private EnergySystem energySystem;


    private void Start()
    {
        userTxt?.SetText(UserData.UserName);
        btnPlay?.onClick.AddListener(() =>
        {
            if(energySystem.HasEnergy)
            {
                energySystem.UsingEnergy();
                UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync((int)SceneIndex.InGame);
            }else
            {
                UIPopUpCtrl.Instance.Get<UINotice>().SetNotice("Out of Energy", "Out of Energy" , () =>
                {
                    UIPopUpCtrl.Instance.Show<UIShop>();
                });
            }
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
           
        });
        btnShop?.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UIPopUpCtrl.Instance.Show<UIShop>();
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
        this.AddListener<GameEvent.OnUserNameChanged>(OnUserNameChanged, false);
    }

    private void OnUserNameChanged(OnUserNameChanged change)
    {
        userTxt?.SetText(UserData.UserName);
    }

}
