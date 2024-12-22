using ShootingGame;
using System;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UIPopUpCtrl : HUD<UIPopUpCtrl>
{
    protected override bool GetDontDestroyOnLoad() { return true; }

    protected override void OnStart()
    {
        base.OnStart();
        HideAllWithout<LoadScene>(false);
        Get<LoadScene>().Initialized();
    }

    public void HideAllWithout<T>(bool anim, Action hideAction = null) where T : ShootingGame.Frame
    {
        //Hide all popups except the one passed in
        activings.Clear();
        Array.ForEach(frames, (f) => { if (f.GetType() != typeof(T)) f.Hide(anim); });
        EndBusy();
        hideAction?.Invoke();
    }



    public override void Show<T>(bool anim = true, Action callback = null, bool hideCurrent = true)
    {
        HideAll(false);
        base.Show<T>(anim, callback, hideCurrent);
    }
}
