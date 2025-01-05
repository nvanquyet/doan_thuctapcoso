using ShootingGame;
using ShootingGame.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_ANDROID
public class ItemShop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
#else
public class ItemShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
#endif
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameItem;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject ownObject;
    [SerializeField] private Button btnBuy;

    public Action<CharacterAttributeData, Vector3> OnHover;
    public Action OnCancelHover;


    private int ID
    {
        get
        {
            return GameData.Instance.Players.GetIndexOfValue(data);
        }
    }

    private CharacterAttributeData data;

    public bool IsOwn
    {
        get
        {
            if (ID < 0) return false;
            if (ID == 0) return true;
            return UserData.GetOwnerCharacter(ID);
        }
        set
        {
            if (ID <= 0) return;
            UserData.SetOwnerCharacter(ID, value);
            btnBuy.gameObject.SetActive(!value);
            ownObject.SetActive(value);
        }
    }

    private void Start()
    {
        this.AddListener<GameEvent.CoinChange>(OnCoinChange, false);   
    }

    private void OnEnable()
    {
        Invoke(nameof(OnCheckBtnBuy), .5f);
    }

    private void OnCoinChange(GameEvent.CoinChange param)
    {
        OnCheckBtnBuy();
    }

    private void OnCheckBtnBuy()
    {
        if (data == null) return;
        if (IsOwn || UserData.CurrentCoin < data.Appearance.Price) btnBuy.interactable = false;
        else btnBuy.interactable = true;
    }

    public void InitData(CharacterAttributeData data, Action onBuy = null)
    {
        if (data == null) return;
        icon.sprite = data.Appearance.Icon;
        nameItem.text = data.Appearance.Name;
        priceText.text = data.Appearance.Price.ToString();
        
        this.data = data;

        btnBuy.gameObject.SetActive(!IsOwn);
        ownObject.SetActive(IsOwn);

        btnBuy.onClick.AddListener(() =>
        {
            SFX.Instance.PlaySound(AudioEvent.ButtonClick);
            UserData.CurrentCoin -= data.Appearance.Price;
            IsOwn = true;
            transform.SetAsLastSibling();
            onBuy?.Invoke();
        });

        OnCheckBtnBuy();
    }
#if UNITY_ANDROID
    public void OnPointerDown(PointerEventData eventData)
    {
        OnHover?.Invoke(data, transform.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnCancelHover?.Invoke();
    }
#else
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover?.Invoke(data, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCancelHover?.Invoke();
    }
#endif
}
