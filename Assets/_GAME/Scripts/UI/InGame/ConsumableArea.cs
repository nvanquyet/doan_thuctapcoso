using ShootingGame;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableArea : MonoBehaviour
{
    [SerializeField] private ConsumableItemButton consumableItemButtonPrefab;

    public void Initialized(List<ItemBuffData> consumableItems, Player target)
    {
        foreach (var item in consumableItems)
        {
            var consumableItemButton = Instantiate(consumableItemButtonPrefab, transform);
            consumableItemButton.Initialized(item, target);
        }
    }
}
