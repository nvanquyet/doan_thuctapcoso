using ShootingGame;
using Unity.VisualScripting;
using UnityEngine;


public class TetrisUI : SingletonBehaviour<TetrisUI>
{
    //create the slots background grid based on inventory size

    TetrisInventory playerInventory;
    [SerializeField] 
    TetrisItemUI itemUIPrefab; //item prefab to create the item UI.

    public TetrisItemUI[,] tetrisItemUIs;
    void Start()
    {
        playerInventory = TetrisInventory.instanceTetris;
        // for (int i = 0; i < playerInventory.numberSlots; i++)
        // {
        //     //var itemUI = Instantiate(slotPrefab, transform);  //generate the slots grid.
        //     var item = Instantiate(itemUIPrefab, transform); //generate the item UI.
        //     tetrisItemUIs[i / 10, i % 10] = item;
        // }
        var line = (int) Mathf.Sqrt(playerInventory.numberSlots);
        tetrisItemUIs = new TetrisItemUI[line, line];
        for (int i = 0; i < line; i++)
        {
            for(int j = 0; j < line ; j++){
                var item = Instantiate(itemUIPrefab, transform); //generate the item UI.
                tetrisItemUIs[i, j] = item;
                item.itemText.SetText("0");
                item.gameObject.SetActive(true);
            }
        }
        itemUIPrefab.gameObject.SetActive(false);
    }
}
