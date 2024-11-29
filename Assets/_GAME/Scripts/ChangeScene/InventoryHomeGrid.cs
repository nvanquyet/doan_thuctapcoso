using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHomeGrid : MonoBehaviour
{
    public GameObject slotPrefab;
    public int totalSlots = 20;  
    public Transform inventoryPanel; 

    void Start()
    {
        CreateSlots();
    }

    void CreateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            Instantiate(slotPrefab, inventoryPanel); 
        }
    }
}
