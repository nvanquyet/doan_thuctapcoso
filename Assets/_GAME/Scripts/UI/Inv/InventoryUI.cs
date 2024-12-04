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

    [SerializeField] private ItemData itemdata1;
    [SerializeField] private ItemData itemData2;
    [SerializeField] private ItemData weaponData1;
    [SerializeField] private ItemData weaponData2;

    [SerializeField] private Player player;






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
        var itemLv1 = itemdata1.GetAllValue();
        var itemLv2 = itemData2.GetAllValue();
        var weaponLv1 = weaponData1.GetAllValue();
        var weaponLv2 = weaponData2.GetAllValue();

        //var item1 = currentStat.GetStat(TypeStat.Luck);
        //Debug.Log(item1);
        
       //ebug.Log(playerStat.CurrentStat.Stats.GetValue(0));
        for (int i = 0; i < maxItem; i++)
        {
            var item = Instantiate(inventoryItemUI, placeHolder);
            item.Initialized(itemLv1[UnityEngine.Random.Range(0, itemLv1.Length - 1)], OnItemClickedAction);
            item.gameObject.SetActive(true);
        }
    }

    private void OnButtonTetrisInvClick() => OnButtonTetrisInvClickAction?.Invoke();

}