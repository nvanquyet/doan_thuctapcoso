using UnityEngine;

public class TetrisInventory : MonoBehaviour
{
    [SerializeField] private int numberSlots = 64;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private TetrisUI tetrisUI;
    [SerializeField] private TetrisSlot tetrisSlot;
    [SerializeField] private WaitingSlots waitingSlots;
#if UNITY_EDITOR
    [SerializeField] private TetrisItem[] itemTest;
    private void OnValidate()
    {
        tetrisUI = GetComponentInChildren<TetrisUI>();
        tetrisUI.SetCellSize(cellSize);
        tetrisSlot = GetComponentInChildren<TetrisSlot>();
        tetrisSlot.SetCellSize(cellSize);
        waitingSlots = GetComponentInChildren<WaitingSlots>();
    }
#endif

    private void Start(){
        SetNumberSlots(this.numberSlots);
    }

    public void SetNumberSlots(int numberSlots)
    {
        this.numberSlots = numberSlots;
        tetrisSlot?.SetGrid(numberSlots);
        tetrisUI?.CreateSlot(numberSlots);
    }

    public void OnAddToTetrisSlot(TetrisItemSlot item)
    {
        waitingSlots.RemoveItem(item);
        tetrisSlot.AddItem(item);
    }

    public void OnReturnToWaitingList(TetrisItemSlot item)
    {
        waitingSlots.AddItem(item);
        tetrisSlot.RemoveItem(item);
    }
#if UNITY_EDITOR
    [ContextMenu("AddItem")]
    private void AddItem()
    {
        for(int i = 0; i < itemTest.Length; i++)
        {
            var item = tetrisSlot.CreateItem(itemTest[i]);
            OnReturnToWaitingList(item);
            item.ActionReturnWaitingList = OnReturnToWaitingList;
            item.ActionAddToBag = OnAddToTetrisSlot;
            item.ActionMarkItemInGrid = tetrisUI.OnMarkItemInGrid;    
        }
    }

#endif


}
