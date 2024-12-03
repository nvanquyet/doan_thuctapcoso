using TMPro;
using UnityEngine;
using UnityEngine.UI;
public interface IBooster
{
    void Use();
}

public class ConsumableItemButton : MonoBehaviour, IBooster
{
    [SerializeField] private Image icon;
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI countText;
    private BuffType buffType;
    private float duration;
    private int amount;

    public void Initialized(ItemBuffData data)
    {
        this.icon.sprite = data.Icon;
        this.buffType = data.BuffType;
        this.amount = data.Amount;
        this.duration = data.Duration;
        countText.text = amount.ToString();
        btn.onClick.AddListener(Use);
    }

    public virtual void Use()
    {
        this.amount--;
        countText.text = amount.ToString();
        btn.interactable = this.amount > 0;
        switch (buffType)
        {
            case BuffType.DamageBoost:
                break;
            case BuffType.SpeedBoost:
                break;
            case BuffType.Heal:
                break;
            case BuffType.ArmorBoost:
                break;
            case BuffType.CriticalChance:
                break;
            case BuffType.DodgeBoost:
                break;
            case BuffType.AttackSpeed:
                break;
            case BuffType.LifeSteal:
                break;
            case BuffType.PoisonImmunity:
                break;
            case BuffType.DamageReduction:
                break;
        }
    }
}
