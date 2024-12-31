using ShootingGame.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title, description;


    public void InitData(Sprite sprite, string title, StatContainerData statContainer)
    {
        this.icon.sprite = sprite;
        string description = string.Empty;
        foreach (var it in statContainer.Stats)
        {
            description += $"{it.GetStatString()}\n";
        }
        this.title.text = title;
        this.description.text = description;
    }
}
