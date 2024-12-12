using DG.Tweening;
using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : Frame
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image icon;
    [SerializeField] private Slider bar;


#if UNITY_EDITOR
    private void OnValidate()
    {
        bar = GetComponentInChildren<Slider>();
        //icon = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
#endif

    public void UpdateProgess(int value, int maxValue)
    {
        text.text = value.ToString() + " / " + maxValue.ToString();
        UpdateProgess((float)value / (float)maxValue);
    }
    public void UpdateProgess(float value)
    {
        bar.DOValue(Mathf.Clamp01(value), 0.25f);
    }

    public void SetIcon(Sprite sprite) => icon.sprite = sprite;
}
