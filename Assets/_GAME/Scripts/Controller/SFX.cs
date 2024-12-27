using UnityEngine;
using ShootingGame;
using DG.Tweening;

public sealed class SFX : MonoBehaviour
{
    public enum SFXLayer
    {
        Background,
        UI,
        Player,
        Other
    }

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

    private void Start()
    {
        PlaySound(AudioEvent.HomeMusic);
    }
    #endregion

    [SerializeField] AudioSource audioBackgroundSource;
    [SerializeField] AudioSource audioUISource;
    [SerializeField] AudioSource audioPlayerSource;
    [SerializeField] AudioSource audioOtherSource;

    SFXData Data => GameData.Instance.SFXData;

    #region Background Music
    public bool MusicEnable
    {
        get => UserData.MusicEnable;
        set
        {
            UserData.MusicEnable = value;
            if (value) ContinueBGM();
            else PauseBGM();
        }
    }

    public void PlayBGM(AudioEvent Event)
    {
        var audioClip = Data.GetAudioClip(Event);
        if (!MusicEnable) return;

        if (audioBackgroundSource.isPlaying)
        {
            if (audioBackgroundSource.clip == audioClip) return;
            audioBackgroundSource.DOKill();
            audioBackgroundSource.DOFade(0, 1).OnComplete(() => PlayBGM(audioClip));
        }
        else PlayBGM(audioClip);
    }

    public void PauseBGM()
    {
        if (audioBackgroundSource)
        {
            audioBackgroundSource.DOKill();
            audioBackgroundSource.DOFade(0, 1).OnComplete(() => audioBackgroundSource.Pause());
        }

    }
    public void ContinueBGM()
    {
        if (!audioBackgroundSource.clip) return;
        PlayBGM(audioBackgroundSource.clip);
    }

    void PlayBGM(AudioClip clip)
    {
        if (clip && audioBackgroundSource)
        {
            audioBackgroundSource.clip = clip;
            audioBackgroundSource.Play();
            audioBackgroundSource.DOKill();
            audioBackgroundSource.DOFade(UserData.MusicVolume, 1);
        }
    }


    #region Movement
    public void PlayPlayerMovement()
    {
        var audioClip = Data.GetAudioClip(AudioEvent.PlayerMovement);
        if (!MusicEnable) return;

        if (!audioPlayerSource.isPlaying)
        {
            PlayPlayerMovement(audioClip);
        }
    }

    void PlayPlayerMovement(AudioClip clip)
    {
        if (clip && audioPlayerSource)
        {
            audioPlayerSource.clip = clip;
            audioPlayerSource.loop = true;
            audioPlayerSource.Play();
            audioPlayerSource.DOKill();
            audioPlayerSource.DOFade(UserData.SoundVolume, .24f);
        }
    }

    public void PausePlayerMovement()
    {
        if (audioPlayerSource && audioPlayerSource.isPlaying)
        {
            audioPlayerSource.loop = false;
            audioPlayerSource.DOKill();
            audioPlayerSource.DOFade(0, .24f).OnComplete(() => audioPlayerSource.Pause());
        }
    }
    #endregion
    #endregion


    #region Sound
    public bool SoundEnable
    {
        get => UserData.SoundEnable;
        set => UserData.SoundEnable = value;
    }

    public void PlaySound(AudioClip clip, SFXLayer layer)
    {
        var source = GetAudioSource(layer);
        if (!SoundEnable || clip == null || !source) return;
        source.PlayOneShot(clip, UserData.SoundVolume);
    }

    private void PlaySound(AudioEvent audioEvent, SFXLayer layer)
       => PlaySound(Data.GetAudioClip(audioEvent), layer);

    private void StopAudio(AudioSource aSource)
    {
        if (!aSource) return;

        aSource.Stop();
        aSource.clip = null;
        aSource.loop = false;
    }

    private AudioSource GetAudioSource(SFXLayer layer)
    {
        switch (layer)
        {
            case SFXLayer.Background: return audioBackgroundSource;
            case SFXLayer.UI: return audioUISource;
            case SFXLayer.Player: return audioPlayerSource;
            case SFXLayer.Other: return audioOtherSource;
            default: return audioOtherSource;
        }
    }
    #endregion

    #region Vibration

    public bool VibrateEnable
    {
        get => PlayerPrefs.GetInt("VibrateEnable", 1) == 1;
        set => PlayerPrefs.SetInt("VibrateEnable", value ? 1 : 0);
    }

    public void VibrateSelection()
    {
#if UNITY_EDITOR
        return;
#else
        if (!VibrateEnable) return;
        try
        {
            Vibration.VibratePop();
        }
        catch { throw; }
#endif
    }

    public void Vibrate(bool heavy = false)
    {
#if UNITY_EDITOR
        return;
#else
        if (!VibrateEnable) return;
        try
        {
            if (!heavy) Vibration.Vibrate();
            else Vibration.VibratePeek();
        }
        catch { throw; }
#endif
    }
    #endregion
    public void PlaySound(AudioEvent audioEvent)
    {
        switch (audioEvent)
        {
            case AudioEvent.ButtonClick:
                PlaySound(audioEvent, SFXLayer.UI);
                break;
            case AudioEvent.VictoryMusic:
            case AudioEvent.LoseMusic:
                PlaySound(audioEvent, SFXLayer.Other);
                break;
            case AudioEvent.PlayerHit:
            case AudioEvent.PlayerShoot:
                PlaySound(audioEvent, SFXLayer.Other);
                break;
            case AudioEvent.InGameMusic:
            case AudioEvent.HomeMusic:
                PlayBGM(audioEvent);
                break;
            default:
                PlaySound(audioEvent, SFXLayer.Other);
                break;
        }
    }
}