using ShootingGame;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : Frame
{
    [SerializeField] private Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            Hide();
        });
    }
}
