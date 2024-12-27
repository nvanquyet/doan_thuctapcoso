using Google.MiniJSON;
using ShootingGame;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIShop : Frame
{
    [Header("Button")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button characterButton;
    [SerializeField] private Button projectileButton;

    [Header("Tab")]
    [SerializeField] private ScrollRect[] tabs;

    [Header("Prefabs")]
    [SerializeField] private ItemShop prefabs;


    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            Hide();
        });
        characterButton.onClick.AddListener(OnClickTabCharacter);
        projectileButton.onClick.AddListener(OnClickTabProjectile);


        InitData();
    }

    private void InitData()
    {
        InitCharacter();
        InitProjectile();
    }

    private void InitProjectile()
    {
        throw new NotImplementedException();
    }

    private void InitCharacter()
    {
        var allCharacter = GameData.Instance.Players.GetAllValue();
        for (int i = 0; i < allCharacter.Length; i++)
        {
            var character = allCharacter[i];
            var item = Instantiate(prefabs, tabs[0].content);
            var isOwn = UserData.GetOwnerCharacter(i, true) || character.IsOwn;
            item.InitData(character.Appearance.Icon, character.Appearance.Name, character.Appearance.Price.ToString(), isOwn, () =>
            {
                SFX.Instance.PlaySound(AudioEvent.ButtonClick);
                UserData.CurrentCoin -= character.Appearance.Price;
                item.transform.SetAsLastSibling();
            });
        }
    }
    private void OnEnable()
    {
        OnClickTabCharacter();
    }

    private void OnClickTabProjectile()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        ActiveTab(0);
    }

    private void ActiveTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].gameObject.SetActive(i == index);
            if(i == index) ScrollTop(tabs[i]);  
        }
    }

    private void OnClickTabCharacter()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        ActiveTab(1);
    }

    private void ScrollTop(ScrollRect rect) => rect.verticalNormalizedPosition = 1;
}
