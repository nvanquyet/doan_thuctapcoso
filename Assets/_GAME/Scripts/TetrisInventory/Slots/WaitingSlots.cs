using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingSlots : MonoBehaviour
{
    private List<TetrisItemSlot> items = new List<TetrisItemSlot>();

    //[SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private ScrollRect scrollView;

#if UNITY_EDITOR 
    private void OnValidate()
    {
        //layoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
        scrollView = GetComponentInChildren<ScrollRect>();
    }
#endif

    public void AddItem(TetrisItemSlot item)
    {
        if(items.Contains(item)) return;
        items.Add(item);
        item.transform.SetParent(scrollView.content);
    }

    public void RemoveItem(TetrisItemSlot item)
    {
        if(!items.Contains(item)) return;
        items.Remove(item);
    }
}
