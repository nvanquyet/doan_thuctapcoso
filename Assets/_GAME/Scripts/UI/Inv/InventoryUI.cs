using ShootingGame;
using ShootingGame.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Frame
{
    [SerializeField] private Button btnTetrisInv;
    [SerializeField] private Button btnRandomItem;

    [SerializeField] private Transform placeHolder;
    [SerializeField] private InventoryItemUI inventoryItemUI;
    [SerializeField] private int maxItem = 5;

    public Action OnButtonTetrisInvClickAction;
    public Action<ItemAttributeData> OnItemClickedAction;

    private void Start()
    {
        btnTetrisInv.onClick.AddListener(OnButtonTetrisInvClick);
        btnRandomItem.onClick.AddListener(OnButtonRandomItemClick);
    }

    public void OnButtonRandomItemClick()
    {
        //Remove all Item in Placeholder
        foreach (Transform child in placeHolder)
        {
            Destroy(child.gameObject);
        }
        //Random Item Here
        var allItems = GameData.Instance.ItemData.GetAllValue();
        for (int i = 0; i < maxItem; i++)
        {
            var item = Instantiate(inventoryItemUI, placeHolder);
            item.Initialized(allItems[UnityEngine.Random.Range(0, allItems.Length - 1)], OnItemClickedAction);
            item.gameObject.SetActive(true);
        }
    }

    private void OnButtonTetrisInvClick() => OnButtonTetrisInvClickAction?.Invoke();
}
