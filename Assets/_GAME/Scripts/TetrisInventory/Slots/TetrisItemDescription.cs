using ShootingGame;
using UnityEngine;
using UnityEngine.UI;

public class TetrisItemDescription : UIComponent
{
    [SerializeField] private Text textTitle;
    [SerializeField] private Image icon;
    [SerializeField] private Text textDescription;

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
