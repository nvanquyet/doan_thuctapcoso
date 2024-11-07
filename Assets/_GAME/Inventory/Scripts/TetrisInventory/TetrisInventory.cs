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

    public void AddItemToTetrisSlot(TetrisItemSlot item)
    {
        waitingSlots.RemoveItem(item);
        tetrisSlot.AddItem(item);
    }

    public void ReturnToWaitingList(TetrisItemSlot item)
    {
        Debug.Log("ReturnToWaitingList");
        waitingSlots.AddItem(item);
        tetrisSlot.RemoveItem(item);
    }
#if UNITY_EDITOR
    private int index = 0;
    [ContextMenu("AddItem")]
    private void AddItem()
    {
        var item = tetrisSlot.CreateItem(itemTest[index % itemTest.Length]);
        ReturnToWaitingList(item);
        item.ActionReturnWaitingList = ReturnToWaitingList;
        item.ActionAddToBag = AddItemToTetrisSlot;
        index++;
    }

#endif


}
