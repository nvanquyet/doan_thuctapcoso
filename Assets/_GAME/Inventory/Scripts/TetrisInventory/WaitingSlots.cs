using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ShootingGame;
using UnityEngine;

public class WaitingSlots : MonoBehaviour
{
    [SerializeField] private Transform[] allPositions;

    private List<TetrisItemSlot> items = new List<TetrisItemSlot>();

    #if UNITY_EDITOR
    private void OnValidate()
    {
        var list = GetComponentsInChildren<Transform>().ToList();
        list.RemoveAt(0);
        allPositions = list.ToArray();
    }
    #endif

    public void AddItem(TetrisItemSlot item)
    {
        if(items.Contains(item)) return;
        items.Add(item);
        item.transform.position = allPositions[items.IndexOf(item)].position;
    }

    public void RemoveItem(TetrisItemSlot item)
    {
        if(!items.Contains(item)) return;
        items.Remove(item);
        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.position = allPositions[i].position;
        }
    }
}
