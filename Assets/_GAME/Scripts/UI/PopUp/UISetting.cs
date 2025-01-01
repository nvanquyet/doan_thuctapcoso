using ShootingGame;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum QualityLevel
{
    Low = 0,
    Medium = 2,
    High = 4
}
public class UISetting : Frame
{
    [SerializeField] private Button closeButton, btnLogout;
    [SerializeField] private Slider musicSlider, soundSlider;
    [SerializeField] private ToggleButton btnSoundEnable, btnMusicEnable;
    [SerializeField] private Toggle[] btnQuality;

    private void Start()
    {
        AssignButtonEvent(closeButton, () =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            Hide();
        });

        AssignButtonEvent(btnLogout, () =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UserData.IsLogin = false;
            UIPopUpCtrl.Instance.Get<LoadScene>().LoadSceneAsync((int)SceneIndex.Login);
        });

        AssignToggleButtonEvent(btnSoundEnable, UserData.SoundEnable, (isOn) =>
        {
            UserData.SoundEnable = isOn;
            soundSlider.interactable = isOn;
        });

        AssignToggleButtonEvent(btnMusicEnable, UserData.MusicEnable, (isOn) =>
        {
            UserData.MusicEnable = isOn;
            musicSlider.interactable = isOn;
        });

        AssignSliderEvent(musicSlider, UserData.MusicVolume, (value) => UserData.MusicVolume = value);
        AssignSliderEvent(soundSlider, UserData.SoundVolume, (value) => UserData.SoundVolume = value);

        for (int i = 0; i < btnQuality.Length; i++)
        {
            AssignQualityToggle(btnQuality[i], i);
        }
    }

    private void AssignButtonEvent(Button button, UnityAction action)
    {
        button.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            action?.Invoke();
        });
    }

    private void AssignToggleButtonEvent(ToggleButton toggleButton, bool initialValue, UnityAction<bool> onToggle)
    {
        toggleButton.Initialize(initialValue, (isOn) =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            onToggle?.Invoke(isOn);
        });
    }

    private void AssignSliderEvent(Slider slider, float initialValue, UnityAction<float> onValueChange)
    {
        slider.value = initialValue;
        slider.onValueChanged.AddListener(onValueChange);
    }

    private void AssignQualityToggle(Toggle toggle, int index)
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                QualitySettings.SetQualityLevel((int)GetQualityLevelFromIndex(index));
            }
        });

        if (index == GetIndexFromQuality((QualityLevel)QualitySettings.GetQualityLevel()))
        {
            toggle.Select();
        }
    }


    public static int GetIndexFromQuality(QualityLevel quality)
    {
        return quality switch
        {
            QualityLevel.Low => 0,
            QualityLevel.Medium => 1,
            QualityLevel.High => 2,
            _ => 0,
        };
    }

    public static QualityLevel GetQualityLevelFromIndex(int index)
    {
        return index switch
        {
            0 => QualityLevel.Low,
            1 => QualityLevel.Medium,
            2 => QualityLevel.High,
            _ => QualityLevel.Low,
        };
    }
}
