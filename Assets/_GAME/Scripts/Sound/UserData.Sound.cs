
using UnityEngine;

public partial class UserData
{
    public static bool MusicEnable
    {
        get => PlayerPrefs.GetInt("MusicEnable", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("MusicEnable", value ? 1 : 0);
        }
    }

    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat("MusicVolume", 1);
        set
        {
            PlayerPrefs.SetFloat("MusicVolume", Mathf.Clamp01(value));
        }
    }

    public static bool SoundEnable
    {
        get => PlayerPrefs.GetInt("SoundEnable", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("SoundEnable", value ? 1 : 0);
        }
    }

    public static float SoundVolume
    {
        get => PlayerPrefs.GetFloat("SoundVolume", 1);
        set
        {
            PlayerPrefs.SetFloat("SoundVolume", Mathf.Clamp01(value));
        }
    }
   
}
