using System.Collections;
using System.Collections.Generic;
using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VawnWuyest.Data;

public class TestStat : MonoBehaviour
{
    [SerializeField] private ItemData item;
    public ShootingGame.Player player;
    public TextMeshProUGUI items;
    public Button btn;

    public Transform[] allTs;

    private void Start()
    {
        btn.onClick.AddListener(TestStatData);
    }

    public void TestStatData()
    {
        items.text = "";
        player.Stat.BuffStat(item.GetValue(0));
        Debug.Log(item.GetValue(0).Data.AllStats[0].Value);

        foreach (var item in player.Stat.CurrentStat.Data.AllStats)
        {
            var newItem = Instantiate(items, allTs[0]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == VawnWuyest.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        foreach (var item in player.Stat.BaseData.Data.AllStats)
        {
            var newItem = Instantiate(items, allTs[1]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == VawnWuyest.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        items.gameObject.SetActive(false);
    }

}
