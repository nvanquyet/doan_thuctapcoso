using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtStatus;
    [SerializeField] private Color colorOn, colorOff;
    [SerializeField] private Button btn;
    [SerializeField] private bool moveIcon;
    [SerializeField] private float XPositionOn;
    [SerializeField] private float XPositionOff;
    private bool isOn = true;
    private bool busy;


    public Action<bool> OnCallBackAction;

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            IsOn = !IsOn;
        });
    }

    public bool IsOn
    {
        get => isOn;
        set
        {
            if (busy) return;
            isOn = value;
            busy = true;
            UpdateUI();
            OnCallBackAction?.Invoke(isOn);
            Invoke(nameof(ResetBusy), 0.2f);
        }
    }
    private void ResetBusy()
    {
        busy = false;
    }
    private void UpdateUI()
    {
        txtStatus.text = isOn ? "ON" : "OFF";
        icon.DOColor(isOn ? colorOn : colorOff, 0.2f);
        icon.transform.DOLocalMoveX(isOn ? XPositionOn : XPositionOff, 0.2f);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        btn = GetComponentInChildren<Button>();
    }

    [ContextMenu("Get X Pos Off Value")]
    private void GetXPosOffValue()
    {
       XPositionOff = icon.transform.localPosition.x;
    }

    [ContextMenu("Get X Pos On Value")]
    private void GetXPosOnValue()
    {
        XPositionOn = icon.transform.localPosition.x;
    }

#endif



    internal void Initialize(bool v, Action<bool> value)
    {
        IsOn = v;
        this.OnCallBackAction = value;
    }


}
