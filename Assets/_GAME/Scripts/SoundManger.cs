using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Components")]
    public AudioSource audioFx;       // Nguồn phát hiệu ứng âm thanh
    public AudioSource backgroundMusic; // Nguồn phát nhạc nền

    [Header("Audio Clips")]
    //public AudioClip buttonClip;
    public AudioClip settingClip;
    private bool isSoundOn = true;    // Trạng thái âm thanh

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo không bị hủy khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject); // Xóa bản thể dư thừa
        }
    }

    private void OnValidate()
    {
        // Đảm bảo AudioSource được thiết lập
        if (audioFx == null)
        {
            audioFx = gameObject.AddComponent<AudioSource>();
            audioFx.playOnAwake = false;
        }
    }
    public void SettingClip()
    {
        audioFx.PlayOneShot(settingClip);
    }
}