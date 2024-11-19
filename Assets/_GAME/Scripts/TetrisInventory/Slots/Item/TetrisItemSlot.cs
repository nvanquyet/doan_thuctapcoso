using System;
using System.Collections.Generic;
using ShootingGame;
using ShootingGame.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TetrisItemSlot : UIComponent, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TetrisUpgradeItem tetrisUpgradeItem;
    public ItemAttributeData itemData { get; private set; }
#region Properties
    private TetrisSlot slots;
    private TetrisItemDescription tetrisDescription;
    private Vector2 startPosition, oldPosition, cellSize, distaceToMousePosition;
    private int currentRotation = 0;
    private bool isHolding = false;
#endregion

#region  Action
    private Action<TetrisItemSlot> ActionReturnWaitingList, ActionAddToBag, ActionRemoveItem;
    private Action<int, int, int> ActionMarkItemInGrid;
    //private Action<Sprite, string, string> ActionPointerEnter;
#endregion

#if UNITY_EDITOR
    private void OnValidate()
    {
        tetrisUpgradeItem = GetComponentInChildren<TetrisUpgradeItem>();
    }
#endif

    private void Update()
    {
        if (isHolding && Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }
    }


#region  InitItem
    public void InitItem(TetrisSlot slots, TetrisItemDescription tetrisDescription, ItemAttributeData data, Vector2 cellSize)
    {
        if (data == null) return;
        this.slots = slots;
        this.tetrisDescription = tetrisDescription;
        this.cellSize = cellSize;


        this.itemData = data;
        this.icon.sprite = data.Appearance.Icon;

        RescalingItem(RectTransform);
        RectTransform.anchorMin = new Vector2(0f, 1f);
        RectTransform.anchorMax = new Vector2(0f, 1f);
        RectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public void InitAction(Action<TetrisItemSlot> actionReturnWaitingList, Action<TetrisItemSlot> actionAddToBag, 
                            Action<int, int, int> actionMarkItemInGrid, Action<TetrisItemSlot> actionUpgradeItem, 
                            Action<TetrisItemSlot> actionRemoveItem)
    {
        ActionReturnWaitingList = actionReturnWaitingList;
        ActionAddToBag = actionAddToBag;
        ActionMarkItemInGrid = actionMarkItemInGrid;
        ActionRemoveItem = actionRemoveItem;
        tetrisUpgradeItem.Init(this, actionUpgradeItem);
    }

    private void ShowItemDescription(ItemAttributeData data, bool show)
    {
        tetrisDescription.ActiveDescription(show);
        if(show == false) return;
        var stringStats = "";
        foreach (var stat in data.Stat.Stats)
        {
            stringStats += $"{stat.GetStatString()}\n";
        }
        tetrisDescription.ModifyDescription(data.Appearance.Icon, data.Appearance.name, stringStats);     
    }

#endregion

#region  PointerEvents
    public void OnPointerEnter(PointerEventData eventData) // shows item description
    {
        ShowItemDescription(itemData, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowItemDescription(itemData, false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(slots.transform);
        oldPosition = RectTransform.anchoredPosition;
        distaceToMousePosition = eventData.position - (Vector2)transform.position;
        isHolding = true; // disable registering hit on item
        //Reset the grid to 0
        var itemSize = GameService.GetItemSize(itemData.MatrixData.ItemSize, currentRotation);
        var grid = GameService.GetMatrix(itemData.MatrixData.Matrix, currentRotation);
        ResetGrid(itemSize, grid);
        ActionAddToBag?.Invoke(this);
    }

    private void RotateItem()
    {
        currentRotation -= 90;
        if (currentRotation <= -360)
        {
            currentRotation = 0;
        }
        transform.Rotate(new Vector3(0, 0, -90), Space.World);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - distaceToMousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) => OnEndDrag();
#endregion
   
#region Grid
    private Vector2 AnchorGrid(Vector2 anchorPos, Vector2 halfSize)
    {
        Vector2 finalSlot;
        finalSlot.x = Mathf.Round((anchorPos.x - (halfSize.x * cellSize.x)) / cellSize.x);
        finalSlot.y = Mathf.Round(-(anchorPos.y + (halfSize.y * cellSize.y)) / cellSize.y);
        return finalSlot;
    }

    private bool CheckItemFit(Vector2 finalSlot, Vector2 itemSize, List<Vector2> newPosItem, int[,] grid)
    {
        grid = GameService.GetMatrix(itemData.MatrixData.Matrix, currentRotation);
        for (int sizeY = 0; sizeY < itemSize.y; sizeY++)
        {
            for (int sizeX = 0; sizeX < itemSize.x; sizeX++)
            {
                int checkX = (int)finalSlot.x + sizeX;
                int checkY = (int)finalSlot.y + sizeY;
                if (slots.grid[checkY, checkX] == 1 && grid[sizeY, sizeX] == 1)
                {
                    newPosItem.Clear();
                    return false;
                }
                newPosItem.Add(new Vector2(checkX, checkY));
            }
        }
        return true;
    }

    private bool IsValidPosition(Vector2 finalSlot, Vector2 itemSize)
    {
        return ((int)(finalSlot.x) + (int)itemSize.x - 1) < slots.maxGridX &&
           ((int)(finalSlot.y) + (int)itemSize.y - 1) < slots.maxGridY &&
           (int)finalSlot.x >= 0 && (int)finalSlot.y >= 0;
    }

    public void OnEndDrag()
    {
        var itemSize = GameService.GetItemSize(itemData.MatrixData.ItemSize, currentRotation);
        var halfSize = itemSize / 2;
        var grid = GameService.GetMatrix(itemData.MatrixData.Matrix, currentRotation);
        isHolding = false;
        Vector2 finalSlot = AnchorGrid(RectTransform.anchoredPosition, halfSize); //position that the item was dropped on canvas

        if (!itemData.MatrixData.IsUniformMatrix) this.transform.SetAsFirstSibling();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Check if the item is upgradeable
            if (tetrisUpgradeItem.CheckInteract()) return;

            //Calculate the final slot of the item
            if (IsValidPosition(finalSlot, itemSize)) // test if item is inside slot area
            {
                List<Vector2> newPosItem = new List<Vector2>(); //new item position in bag
                bool fit = CheckItemFit(finalSlot, itemSize, newPosItem, grid); //check if item fits in the bag
                //Caclate the new position of the item
                if (fit)
                {
                    MarkItemInGrid(newPosItem[0], itemSize, grid);
                    startPosition = newPosItem[0];
                    RectTransform.anchoredPosition = new Vector2((newPosItem[0].x + halfSize.x) * cellSize.x, -(newPosItem[0].y + halfSize.y) * cellSize.y);
                    return;
                }
                else
                {
                    if (IsValidPosition(AnchorGrid(oldPosition, halfSize), itemSize))
                    {
                        RectTransform.anchoredPosition = oldPosition;
                        MarkItemInGrid(AnchorGrid(oldPosition, halfSize), itemSize, grid);
                    }
                    else ReturnToWaitingList();
                }
            }
            else
            {
                ReturnToWaitingList();
                return;
            }
        }
        else
        {
            if (IsValidPosition(AnchorGrid(oldPosition, halfSize), itemSize))
            {
                RectTransform.anchoredPosition = oldPosition;
                MarkItemInGrid(AnchorGrid(oldPosition, halfSize), itemSize, grid);
            }
            else ReturnToWaitingList();
        }

    }

    #endregion

    #region  Other
    private void RescalingItem(RectTransform rect)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemData.MatrixData.ItemSize.y * cellSize.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemData.MatrixData.ItemSize.x * cellSize.x);
    }
    
    private void ResetGrid(Vector2 itemSize, int[,] grid)
    {
        for (int i = 0; i < itemSize.y; i++)
        {
            for (int j = 0; j < itemSize.x; j++)
            {
                if (slots.grid[(int)startPosition.y + i, (int)startPosition.x + j] == 1 && grid[i, j] == 1)
                {
                    slots.grid[(int)startPosition.y + i, (int)startPosition.x + j] = 0;
                    ActionMarkItemInGrid?.Invoke((int)startPosition.x + j, (int)startPosition.y + i, 0);
                }
            }
        }
    }

    private void MarkItemInGrid(Vector2 posItem, Vector2 itemSize, int[,] grid)
    {
        for (int i = 0; i < itemSize.y; i++)
        {
            for (int j = 0; j < itemSize.x; j++)
            {
                if (grid[i, j] != 0)
                {
                    slots.grid[(int)posItem.y + i, (int)posItem.x + j] = grid[i, j];
                    ActionMarkItemInGrid?.Invoke((int)posItem.x + j, (int)posItem.y + i, grid[i, j]);
                }
            }
        }
    }


    private void ReturnToWaitingList()
    {
        ActionReturnWaitingList?.Invoke(this);
    }

    internal void OnDestroyItem()
    {
        ActionRemoveItem?.Invoke(this);
        if (transform.parent.Equals(slots.transform))
        {
            var itemSize = GameService.GetItemSize(itemData.MatrixData.ItemSize, currentRotation);
            var grid = GameService.GetMatrix(itemData.MatrixData.Matrix, currentRotation);
            ResetGrid(itemSize, grid);
        }
        //Remove from bag
    }

    #endregion

}