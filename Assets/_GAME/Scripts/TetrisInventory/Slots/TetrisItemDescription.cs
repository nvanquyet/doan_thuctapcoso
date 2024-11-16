using ShootingGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TetrisItemDescription : UIComponent
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI textDescription;

    [SerializeField] private GameObject placeHolder;

    public void ModifyDescription(Sprite icon, string title, string description)
    {
        this.icon.sprite = icon;
        textTitle.text = title;
        textDescription.text = description;
    }

    public void ActiveDescription(bool active)
    {
        placeHolder.SetActive(active);
    }

}
