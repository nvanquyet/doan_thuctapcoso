using System;
using System.Collections.Generic;
using ShootingGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TetrisItemSlot : UIComponent, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<TetrisItemSlot> ActionReturnWaitingList, ActionAddToBag;
    public Action<int, int, int> ActionMarkItemInGrid;
    [SerializeField] private Image icon;
    private TetrisSlot slots;
    private TetrisItem item;
    private Vector2 startPosition, oldPosition, cellSize, distaceToMousePosition;
    private int currentRotation = 0;
    private bool isHolding = false;

    public void OnPointerEnter(PointerEventData eventData) // shows item description
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.itemName);
        // string title = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.itemName;
        // string body = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.itemDescription;
        // int attributte1 = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.getAtt1();
        // Sprite icon_attribute = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.getAtt1Icon();
        // string rarity = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<TetrisItemSlot>().item.rarity;
        // Functionalities descript = FindObjectOfType<Functionalities>();
        // descript.changeDescription(title, body, attributte1, rarity, icon_attribute);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Functionalities descript = FindObjectOfType<Functionalities>();
        // descript.changeDescription("", "", 0, "");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = RectTransform.anchoredPosition;
        distaceToMousePosition = eventData.position - (Vector2)transform.position;
        isHolding = true; // disable registering hit on item
        //Reset the grid to 0
        var itemSize = GameService.GetItemSize(item.matrixData.ItemSize, currentRotation);
        var grid = GameService.GetMatrix(item.matrixData.Matrix, currentRotation);
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


    private void Update()
    {
        if (isHolding && Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - distaceToMousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var itemSize = GameService.GetItemSize(item.matrixData.ItemSize, currentRotation);
        var halfSize = itemSize / 2;
        var grid = GameService.GetMatrix(item.matrixData.Matrix, currentRotation);
        isHolding = false;
        Vector2 finalSlot = AnchorGrid(RectTransform.anchoredPosition, halfSize); //position that the item was dropped on canvas

        if(!item.matrixData.IsUniformMatrix) this.transform.SetAsFirstSibling();

        if (EventSystem.current.IsPointerOverGameObject())
        {
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

    private Vector2 AnchorGrid(Vector2 anchorPos, Vector2 halfSize)
    {
        Vector2 finalSlot;
        finalSlot.x = Mathf.Round((anchorPos.x - (halfSize.x * cellSize.x)) / cellSize.x);
        finalSlot.y = Mathf.Round(-(anchorPos.y + (halfSize.y * cellSize.y)) / cellSize.y);
        return finalSlot;
    }

    private bool CheckItemFit(Vector2 finalSlot, Vector2 itemSize, List<Vector2> newPosItem, int[,] grid)
    {
        grid = GameService.GetMatrix(item.matrixData.Matrix, currentRotation);
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

    private bool IsValidPosition(Vector2 finalSlot, Vector2 itemSize)
    {
        return ((int)(finalSlot.x) + (int)itemSize.x - 1) < slots.maxGridX &&
           ((int)(finalSlot.y) + (int)itemSize.y - 1) < slots.maxGridY &&
           (int)finalSlot.x >= 0 && (int)finalSlot.y >= 0;
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

    private void ReturnToWaitingList()
    {
        ActionReturnWaitingList?.Invoke(this);
    }


    public void InitItem(TetrisSlot slots, TetrisItem tetrisItem, Vector2 cellSize)
    {
        if (tetrisItem == null) return;
        this.slots = slots;
        this.cellSize = cellSize; 

        this.item = tetrisItem;
        this.icon.sprite = tetrisItem.itemIcon;

        RescalingItem(RectTransform);
        RectTransform.anchorMin = new Vector2(0f, 1f);
        RectTransform.anchorMax = new Vector2(0f, 1f);
        RectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    private void RescalingItem(RectTransform rect)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.matrixData.ItemSize.y * cellSize.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.matrixData.ItemSize.x * cellSize.x);

        foreach (RectTransform child in rect.transform)
        {
            child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.matrixData.ItemSize.y * child.rect.height);
            child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.matrixData.ItemSize.x * child.rect.width);

            foreach (RectTransform iconChild in child)
            {
                iconChild.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.matrixData.ItemSize.y * iconChild.rect.height);
                iconChild.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.matrixData.ItemSize.x * iconChild.rect.width);
                iconChild.localPosition = new Vector2(child.localPosition.x + child.rect.width / 2, child.localPosition.y + child.rect.height / 2 * -1f);
            }
        }
    }

}