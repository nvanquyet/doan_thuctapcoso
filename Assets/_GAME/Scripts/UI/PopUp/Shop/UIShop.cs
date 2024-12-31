using ShootingGame;
using ShootingGame.Data;
using System;
using UnityEngine;
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
    [SerializeField] private ItemShopInfo infoItem;


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
        infoItem.gameObject.SetActive(false);
    }

    private void InitData()
    {
        InitCharacter();
        InitProjectile();
    }

    private void InitProjectile()
    {
        
    }

    private void OnHoverItem(CharacterAttributeData data, Vector3 position)
    {
        if (data == null) return;
        if (infoItem == null) return;

        infoItem.InitData(data.Appearance.Icon, data.Appearance.Name, data.Stat);
        infoItem.gameObject.SetActive(true);
        infoItem.transform.position = position;
    }

    private void OnCancelHover()
    {
        if (infoItem == null) return;
        infoItem.gameObject.SetActive(false);
    }

    private void InitCharacter()
    {
        var allCharacter = GameData.Instance.Players.GetAllValue();
        var allItem = new ItemShop[allCharacter.Length];
        for (int i = 0; i < allCharacter.Length; i++)
        {
            var character = allCharacter[i];
            var item = Instantiate(prefabs, tabs[0].content);
            var idCharacter = GameData.Instance.Players.GetIndexOfValue(character);
            item.InitData(character, () =>
            {
                GameService.LogColor($"Buy Character {character.Appearance.Name} Index {idCharacter} Success");
            });
            item.OnHover += OnHoverItem;
            item.OnCancelHover += OnCancelHover;
            allItem[i] = item;
        }
        foreach (var item in allItem)
        {
            if (item.IsOwn) item.transform.SetAsLastSibling();
        }
    }
    private void OnEnable()
    {
        OnClickTabCharacter();
    }

    private void OnClickTabProjectile()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        ActiveTab(1);
    }

    private void ActiveTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].gameObject.SetActive(i == index);
            if (i == index) ScrollTop(tabs[i]);
        }
    }

    private void OnClickTabCharacter()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        ActiveTab(0);
    }

    private void ScrollTop(ScrollRect rect) => rect.verticalNormalizedPosition = 1;
}
