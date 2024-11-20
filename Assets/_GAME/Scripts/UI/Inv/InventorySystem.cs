
using System.Linq;
using ShootingGame;
using UnityEngine;
using UnityEngine.UI;
public class InventorySystem : Frame
{
    [SerializeField] private Button buttonPlayGame;
    [SerializeField] private Button buttonSpawnItem;

    [SerializeField] private TetrisInventory tetrisInventory;


    #if UNITY_EDITOR
    private void OnValidate()
    {
        tetrisInventory = GetComponentInChildren<TetrisInventory>();
    }
    #endif

    private void Start()
    {
        buttonPlayGame.onClick.AddListener(OnButtonPlayGameClick);
        buttonSpawnItem.onClick.AddListener(OnButtonSpawnItemClick);
        this.AddListener<GameEvent.OnWaveClear>(OnWaveClear, false);
    }

    private void OnButtonPlayGameClick()
    {
        //Call next wave
        UICtrl.Instance.Get<InventorySystem>().Hide(true, () => {
            GameCtrl.Instance.NextWave();
            var allWeaponIds = tetrisInventory.GetTetrisItemsID();
            this.Dispatch<GameEvent.OnNextWave>(new GameEvent.OnNextWave { allIDItem = allWeaponIds.ToList() });
        });
    }

    private void OnWaveClear()
    {
        UICtrl.Instance.Get<InventorySystem>().Show();
    }

    private void OnButtonSpawnItemClick()
    {
        tetrisInventory.AddItem();
    }
}
