
using ShootingGame;
using UnityEngine;

public class UICtrl : HUD<UICtrl>
{
    private RectTransform rectTransform;

    public RectTransform RectTransform
    {
        get
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }
    }
}
