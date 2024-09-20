using System.Collections;
using System.Collections.Generic;
using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestStat : MonoBehaviour
{
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
        

        foreach (var item in player.Stat.AllStats.AllProperties)
        {
            var newItem = Instantiate(items, allTs[0]);
            newItem.text += item.StatDefine + " : " + item.Value + "\n";
            newItem.gameObject.SetActive(true);
        }
        foreach (var item in player.Stat.TotalStats.AllProperties)
        {
            var newItem = Instantiate(items, allTs[1]);
            newItem.text += item.StatDefine + " : " + item.Value + "\n";
            newItem.gameObject.SetActive(true);
        }
        items.gameObject.SetActive(false);
    }

}
