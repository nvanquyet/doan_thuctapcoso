using UnityEngine;
using ShootingGame;

public partial class SFX : MonoBehaviour
{


    #region Singleton
    private static SFX instance;

    public static SFX Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<SFX>();
            if (instance == null)
            {
                var obj = Resources.Load<GameObject>($"{typeof(SFX).Name}/{typeof(SFX).Name}");
                if (obj != null)
                {
                    instance = Instantiate(obj).GetComponent<SFX>();
                }
                else
                {
                    instance = new GameObject(typeof(SFX).Name).AddComponent<SFX>();
                    instance.gameObject.AddComponent<AudioListener>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance.GetInstanceID() != this.GetInstanceID())
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Vibration.Init();
    }

    #endregion
    [SerializeField] AudioSource audioSource;
    SFXData Data => GameData.Instance.SFXData;

    public bool SoundEnable
    {
        get => PlayerPrefs.GetInt("SoundEnable", 1) == 1;
        set => PlayerPrefs.SetInt("SoundEnable", value ? 1 : 0);
    }

    public bool VibrateEnable
    {
        get => PlayerPrefs.GetInt("VibrateEnable", 1) == 1;
        set => PlayerPrefs.SetInt("VibrateEnable", value ? 1 : 0);
    }

    public void VibrateSelection()
    {
#if UNITY_EDITOR
        return;
#endif
        if (!VibrateEnable) return;
        try
        {
            Vibration.VibratePop();
        }
        catch { throw; }
    }

    public void Vibrate(bool heavy = false)
    {
#if UNITY_EDITOR
        return;
#endif
        if (!VibrateEnable) return;
        try
        {
            if (!heavy) Vibration.Vibrate();
            else Vibration.VibratePeek();
        }
        catch { throw; }
    }

    public void PlaySound(AudioEvent audioEvent, float volume = 1)
        => PlaySound(Data.GetAudioClip(audioEvent), volume);

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        if (!SoundEnable || clip == null || !audioSource) return;
        audioSource.PlayOneShot(clip, volume);
    }

    public void StopAudio()
    {
        if (!audioSource) return;

        audioSource.Stop();
        audioSource.clip = null;
        audioSource.loop = false;
    }
}