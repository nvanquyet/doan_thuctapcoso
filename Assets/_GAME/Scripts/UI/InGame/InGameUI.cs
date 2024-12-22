using ShootingGame;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : Frame
{
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private ProgressBar bossProgessBar;

    [SerializeField] private Button btnSetting;

    public void SetBossProgess(float value)
    {
        if (bossProgessBar) bossProgessBar.UpdateProgess(value);
    }

    public void ActiveBossProgess(bool active = true, bool resetProgess = false)
    {
        if (bossProgessBar) bossProgessBar.gameObject.SetActive(active);
        if (resetProgess) bossProgessBar.UpdateProgess(1);
    }

    public void SetIconBoss(Sprite sprite)
    {
        if (bossProgessBar) bossProgessBar.SetIcon(sprite);
    }

    public void SetWaveText(int wave) => waveText.text = $"Wave {wave}";
    public void SetEnemiesKilledText(int killed)
    {
        string s = "";
        if (killed < 10) s = $"000{killed}";
        else if (killed < 100) s = $"00{killed}";
        else if (killed < 1000) s = $"0{killed}";
        else s = $"{killed}";
        enemiesKilledText.text = s;
    }
    private void Start()
    {
        this.AddListener<GameEvent.OnNextWave>(() =>
        {
            UICtrl.Instance.Show<InGameUI>();
        }, false);

        SetEnemiesKilledText(0);
        btnSetting?.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UIPopUpCtrl.Instance.Show<UISetting>();
        });
        gameObject.SetActive(false);
    }

    internal void SetCoin(int coinClaimed) => coinText.text = coinClaimed.ToString();
}
