
using ShootingGame;
using UnityEngine;
using UnityEngine.UI;
using VawnWuyest;

public class InventorySystem : Frame
{
    [SerializeField] private Button buttonPlayGame;
    [SerializeField] private Button buttonSpawnItem;



    private void Start()
    {
        buttonPlayGame.onClick.AddListener(OnButtonPlayGameClick);
        buttonSpawnItem.onClick.AddListener(OnButtonSpawnItemClick);
        this.AddListener<GameEvent.OnWaveClear>(OnWaveClear, false);
    }

    private void OnButtonPlayGameClick()
    {
        //Call next wave
        UICtrl.Instance.Get<InventorySystem>().Hide(true, () => GameCtrl.Instance.NextWave());
    }

    private void OnWaveClear()
    {
        UICtrl.Instance.Get<InventorySystem>().Show();
    }

    private void OnButtonSpawnItemClick()
    {
        GameCtrl.Instance.SpawnItem();
    }
}
