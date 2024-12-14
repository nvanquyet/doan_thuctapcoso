using ShootingGame;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableArea : MonoBehaviour
{
    [SerializeField] private ConsumableItemButton consumableItemButtonPrefab;
    [SerializeField] private Player target;

#if UNITY_EDITOR
    private void OnValidate()
    {
        target = FindObjectOfType<Player>();
    }
#endif


    private void Awake()
    {
        this.AddListener<GameEvent.OnNextWave>(OnNextWave, false);
    }

    private void OnNextWave(GameEvent.OnNextWave param)
    {
        GameService.LogColor($"ConsumableArea OnNextWave {param.allBuffItems.Count}");
        if (param.allBuffItems != null && param.allBuffItems.Count > 0)
        {
            Initialized(param.allBuffItems, param.player);
        }
    }

    public void Initialized(List<ItemBuffData> consumableItems, Player target)
    {
        foreach (var item in consumableItems)
        {
            var consumableItemButton = Instantiate(consumableItemButtonPrefab, transform);
            consumableItemButton.Initialized(item, target);
            consumableItemButton.gameObject.SetActive(true);
        }
    }
}
