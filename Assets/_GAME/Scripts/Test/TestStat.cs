using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ShootingGame.Data;

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
        player.Stat.BuffStat(item.GetValue(1));
        Debug.Log(item.GetValue(0).Data.Stats[0].Value);

        foreach (var item in player.Stat.CurrentStat.Data.Stats)
        {
            var newItem = Instantiate(items, allTs[0]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == ShootingGame.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        foreach (var item in player.Stat.BaseData.Data.Stats)
        {
            var newItem = Instantiate(items, allTs[1]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == ShootingGame.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        items.gameObject.SetActive(false);
    }

}
