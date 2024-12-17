using DG.Tweening;
using ShootingGame;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UINotice : Frame
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMPro.TextMeshProUGUI title;
    [SerializeField] private TMPro.TextMeshProUGUI description;

    private Action OnCallBack;

    private void Start()
    {
        closeButton.onClick.AddListener(HidePopUp);
    }

    public void SetNotice(string title, string description, Action callBack = null)
    {
        this.title.text = title;
        this.description.text = description;
        Show();
        Invoke(nameof(HidePopUp), 3f);
        OnCallBack = callBack;
    }

    private void HidePopUp()
    {
        Hide(true, OnCallBack);
    }

    private void OnDisable()
    {
        OnCallBack = null;
    }
}
