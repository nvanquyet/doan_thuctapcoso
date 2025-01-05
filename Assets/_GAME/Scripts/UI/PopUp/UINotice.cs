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
        this.AddListener<GameEvent.OnReceiveNotice>(OnReceiveNotice, false);
    }

    private void OnReceiveNotice(GameEvent.OnReceiveNotice notice)
    {
        SetNotice("Notice", notice.message, notice.callBack);
    }

    public void SetNotice(string title, string description, Action callBack = null)
    {
        if(!this.title || !this.description) return;
        if(this.title) this.title.text = title;
        if(this.description) this.description.text = description;
        Show();
        Invoke(nameof(HidePopUp), 3f);
        OnCallBack = callBack;
    }
 
    private void HidePopUp() 
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        Hide(true, OnCallBack);
    }

    private void OnDisable()
    {
        OnCallBack = null;
    }
}
