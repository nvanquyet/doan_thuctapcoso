using ShootingGame;
using UnityEngine;
using UnityEngine.UI;

//public class TetrisUI : MonoBehaviour
public class TetrisUI : SingletonBehaviour<TetrisUI>
{
    [SerializeField]
    private TetrisItemUI itemUIPrefab;

    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;


#if UNITY_EDITOR
    private void OnValidate()
    {
        itemUIPrefab = GetComponentInChildren<TetrisItemUI>();
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
    }
#endif

    public TetrisItemUI[,] tetrisItemUIs { get; private set; }

    public void CreateSlot(int numberSlots)
    {
        itemUIPrefab.transform.SetParent(transform.parent);
        foreach (Transform child in transform)
        {
            Debug.Log("Destroy: " + child.name);
            Destroy(child.gameObject);
        }
        var line = (int)Mathf.Sqrt(numberSlots);
        tetrisItemUIs = new TetrisItemUI[line, line];
        for (int i = 0; i < line; i++)
        {
            for (int j = 0; j < line; j++)
            {
                var item = Instantiate(itemUIPrefab, transform); //generate the item UI.
                tetrisItemUIs[i, j] = item;
                item.OnMarkItem(false);
            }
        }
        itemUIPrefab.gameObject.SetActive(false);

        SetContrains(numberSlots);

        Debug.Log("CreateSlot: " + line + "x" + line);
    }

    public void SetCellSize(Vector2 cellSize)
    {
        gridLayoutGroup.cellSize = cellSize;
    }
    private void SetContrains(int numberSlots)
    {
        gridLayoutGroup.constraintCount = (int)Mathf.Sqrt(numberSlots);
    }

    public void OnMarkItemInGrid(int x, int y, int value)
    {
        tetrisItemUIs[y, x].OnMarkItem(value > 0);
    }
}
