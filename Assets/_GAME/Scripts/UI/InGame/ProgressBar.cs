using DG.Tweening;
using ShootingGame;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : Frame
{
    [SerializeField] private Text text;
    [SerializeField] private Image icon;
    [SerializeField] private Slider bar;


#if UNITY_EDITOR
    private void OnValidate()
    {
        bar = GetComponentInChildren<Slider>();
        icon = GetComponent<Image>();
    }
#endif

    public void UpdateProgess(int value, int maxValue)
    {
        text.text = value.ToString() + " / " + maxValue.ToString();
        bar.DOValue(Mathf.Clamp01((float)value / (float)maxValue), 0.25f);
    }
    
    public void SetIcon(Sprite sprite) => icon.sprite = sprite;
}
