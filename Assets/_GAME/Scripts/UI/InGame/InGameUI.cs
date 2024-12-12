using ShootingGame;
using TMPro;
using UnityEngine;

public class InGameUI : Frame
{
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI waveText;

    public void SetWaveText(int wave) => waveText.text = $"Wave {wave}";
    public void SetEnemiesKilledText(int killed)
    {
        string s = "";
        if (killed < 10) s = $"Wave 000{killed}";
        else if (killed < 100) s = $"Wave 00{killed}";
        else if (killed < 1000) s = $"Wave 0{killed}";
        else s = $"Wave {killed}";
        enemiesKilledText.text = s;
    }
    private void Start()
    {
        this.AddListener<GameEvent.OnNextWave>(() =>
        {
            UICtrl.Instance.Show<InGameUI>();
        }, false);
        gameObject.SetActive(false);
    }




}
