using ShootingGame;
using ShootingGame.Data;
using System.Collections.Generic;
using UnityEngine;


public class TetrisSlot : UIComponent
{
    #region  Properties Matrix
    public int[,] grid { get; private set; }
    public int maxGridX { get; private set; }
    public int maxGridY { get; private set; }

    [SerializeField] private Vector2 cellSize;
    [SerializeField] private TetrisItemSlot prefabSlot;
    
    #endregion

    private List<TetrisItemSlot> itemsInBag = new List<TetrisItemSlot>();

    public List<TetrisItemSlot> ItemsInBag => itemsInBag;

    public void SetGrid(int numberSlots)
    {
        if(grid == null) {
            maxGridX = (int)Mathf.Sqrt(numberSlots);
            maxGridY = (int)Mathf.Sqrt(numberSlots);
            grid = new int[maxGridY, maxGridX];
        }
        else {
            if(maxGridX * maxGridY >= numberSlots) return;
            int[,] newGrid = new int[maxGridY, maxGridX];
            for (int i = 0; i < maxGridY; i++)
            {
                for (int j = 0; j < maxGridX; j++)
                {
                    newGrid[i, j] = grid[i, j];
                }
            }
            maxGridX = (int)Mathf.Sqrt(numberSlots);
            maxGridY = (int)Mathf.Sqrt(numberSlots);
            grid = newGrid;
        } 
        Debug.Log("SetGrid: " + maxGridX + "x" + maxGridY);
    }

    public TetrisItemSlot CreateItem(TetrisItemDescription tetrisDescription, ItemDataSO item)
    {
        TetrisItemSlot myItem = Instantiate(prefabSlot, transform);
        myItem.InitItem(this, tetrisDescription, item, cellSize);
        myItem.gameObject.SetActive(true);
        return myItem;
    }

    public void RemoveItem(TetrisItemSlot item)
    {
        if (itemsInBag.Contains(item)) itemsInBag.Remove(item);
    }

    public void AddItem(TetrisItemSlot item)
    {
        if(!itemsInBag.Contains(item)) itemsInBag.Add(item);
    }

    public void SetCellSize(Vector2 cellSize)
    {
        this.cellSize = cellSize;
    }
}
