using DG.Tweening;
using ShootingGame;
using UnityEngine;
using UnityEngine.UI;

public class UINotice : Frame
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMPro.TextMeshProUGUI title;
    [SerializeField] private TMPro.TextMeshProUGUI description;

    private void Start()
    {
        closeButton.onClick.AddListener(HidePopUp);
    }

    public void SetNotice(string title, string description)
    {
        this.title.text = title;
        this.description.text = description;
        Show();
        Invoke(nameof(HidePopUp), 3f);
    }

    private void HidePopUp() => Hide();
}
