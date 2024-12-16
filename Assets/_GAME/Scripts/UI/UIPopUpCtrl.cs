using ShootingGame;
using System;

public class UIPopUpCtrl : HUD<UIPopUpCtrl>
{
    protected override bool GetDontDestroyOnLoad() { return true; }

    protected override void OnStart()
    {
        base.OnStart();
        HideAll(false);
    }
    public override void Show<T>(bool anim = true, Action callback = null, bool hideCurrent = true)
    {
        HideAll(false);
        base.Show<T>(anim, callback, hideCurrent);
    }
}
