using ShootingGame;
using System;
using UnityEngine;
using UnityEngine.UI;
public enum QualityLevel
{
    Low = 0,
    Medium = 2,
    High = 4
}
public class UISetting : Frame
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button btnSave;
    [SerializeField] private Button btnCancel;
    [SerializeField] private Slider musicSlider, soundSlider;
    [SerializeField] private ToggleButton btnSoundEnable;
    [SerializeField] private ToggleButton btnMusicEnable;
    [SerializeField] private Toggle[] btnQuality;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            Hide();
        });

        btnSoundEnable.Initialize(UserData.SoundEnable, (isOn) =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UserData.SoundEnable = isOn;
            soundSlider.interactable = isOn;
        });

        btnMusicEnable.Initialize(UserData.MusicEnable, (isOn) =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UserData.MusicEnable = isOn;
            musicSlider.interactable = isOn;
        });

        musicSlider.value = UserData.MusicVolume;
        musicSlider.onValueChanged.AddListener((value) =>
        {
            UserData.MusicVolume = value;
        });

        soundSlider.value = UserData.SoundVolume;
        soundSlider.onValueChanged.AddListener((value) =>
        {
            UserData.SoundVolume = value;
        });


        for (int i = 0; i < btnQuality.Length; i++)
        {
            var btn = btnQuality[i];
            btn.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    var index = Array.IndexOf(btnQuality, btn);
                    QualitySettings.SetQualityLevel((int)(GetQualityLevelFromIndex(index)));
                }
            });
            if (i == GetIndexFromQuality((QualityLevel) QualitySettings.GetQualityLevel()))
            {
                btnQuality[i].Select();
            }
        }
    }

    public static int GetIndexFromQuality(QualityLevel quality)
    {
        switch (quality)
        {
           case QualityLevel.Low:
                return 0;
            case QualityLevel.Medium:
                return 1;
            case QualityLevel.High:
                return 2;
            default:
                return 0;
        }
    }

    public static QualityLevel GetQualityLevelFromIndex(int index)
    {
        switch (index)
        {
            case 0:
                return QualityLevel.Low;
            case 1:
                return QualityLevel.Medium;
            case 2:
                return QualityLevel.High;
            default:
                return QualityLevel.Low;
        }
    }
}
