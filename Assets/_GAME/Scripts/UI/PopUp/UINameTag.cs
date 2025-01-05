using ShootingGame;
using UnityEngine;
using UnityEngine.UI;
using static GameEvent;
public class UINameTag : Frame
{
    [SerializeField] private InputText inputText;
    [SerializeField] private Button btnSave;


    private void Start()
    {
        btnSave.onClick.AddListener(OnClickSave);
        this.AddListener<GameEvent.OnLoadPlayer>(OnLoadPlayer, false);
    }

    public void ShowNameTag(string tilte)
    {
        if (!string.IsNullOrEmpty(tilte))
        {
            inputText.SetTitle(tilte);
            Invoke(nameof(ResetTitle), 1f);
        }
    }

    private void OnLoadPlayer(OnLoadPlayer loadPlayer)
    {
        if (loadPlayer.success) UIPopUpCtrl.Instance.Hide<UINameTag>();
    }

    private void ResetTitle() => inputText.SetTitle("Enter your name");

    private void OnClickSave()
    {
        if (string.IsNullOrEmpty(inputText.Text)) return;
        UserData.UserName = inputText.Text;
        Service.gI().CreatePlayer(UserData.UserName);
    }
}
