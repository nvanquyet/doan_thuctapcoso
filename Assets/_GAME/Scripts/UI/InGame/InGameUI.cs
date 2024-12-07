using ShootingGame;
using UnityEngine;

public class InGameUI : Frame
{
    private void Start()
    {
        this.AddListener<GameEvent.OnNextWave>(() =>
        {
            UICtrl.Instance.Show<InGameUI>();
        }, false);
        gameObject.SetActive(false);
    }

}
