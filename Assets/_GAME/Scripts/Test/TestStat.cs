using TMPro;
using UnityEngine;
using ShootingGame.Data;

public class TestStat : MonoBehaviour
{
    public TextMeshProUGUI items;

    public Transform tsBase, tsCurrent;

    public void ShowStat(ShootingGame.Player player)
    {
        ShowBaseStat(player);
        ShowCurrentStat(player);
    }

    private void ShowCurrentStat(ShootingGame.Player player)
    {
        ShowStat(tsCurrent, player.Stat.CurrentStat);
    }

    private void ShowBaseStat(ShootingGame.Player player)
    {
        ShowStat(tsBase, player.Stat.BaseData);
    }

    private void ShowStat(Transform parentTs, StatContainerData statContainer)
    {
        foreach (Transform ts in parentTs)
        {
            Destroy(ts.gameObject);
        }
        foreach (var stat in statContainer.Stats)
        {
            var newItem = Instantiate(items, parentTs);
            newItem.text = $"{stat.TypeStat}: {stat.Value}{(stat.TypeValueStat == TypeValueStat.FixedValue ? "" : "%")}";
        }
    }

}
