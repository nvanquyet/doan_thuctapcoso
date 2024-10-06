using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ShootingGame.Data;
using System;
using ShootingGame;

public class TestStat : MonoBehaviour
{
    [SerializeField] private ItemData item;
    public ShootingGame.Player player;
    public WeaponCtrl weaponCtrl;
    public TextMeshProUGUI items;
    public Button btn;
    public Button btnApplyStatForWeapon;

    public Transform[] allTs;

    private void Start()
    {
        btn.onClick.AddListener(TestStatData);
        btnApplyStatForWeapon.onClick.AddListener(ApplyStatForWeapon);
    }

    private void ApplyStatForWeapon()
    {
        var weapon = weaponCtrl.AllWeapons[0];
        var baseStat = weapon.EquiqmentStat.Data.Stats;
        var currentStat = weapon.CurrentEquiqmentStat.Data.Stats;
        foreach (var item in baseStat)
        {
            var newItem = Instantiate(items, allTs[2]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == ShootingGame.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        foreach (var item in currentStat)
        {
            var newItem = Instantiate(items, allTs[3]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == ShootingGame.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        items.gameObject.SetActive(false);


    }

    public void TestStatData()
    {
        items.text = "";
        player.Stat.BuffStat(item.GetValue(0));
        player.Stat.BuffStat(item.GetValue(1));
        foreach (var item in player.Stat.BaseData.Data.Stats)
        {
            var newItem = Instantiate(items, allTs[1]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == ShootingGame.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        foreach (var item in player.Stat.CurrentStat.Data.Stats)
        {
            var newItem = Instantiate(items, allTs[0]);
            newItem.text += item.TypeStat + " : " + item.Value + (item.TypeValueStat == ShootingGame.Data.TypeValueStat.Percentage ? "%" : "") + "\n";
            newItem.gameObject.SetActive(true);
        }
        items.gameObject.SetActive(false);
    }

}
