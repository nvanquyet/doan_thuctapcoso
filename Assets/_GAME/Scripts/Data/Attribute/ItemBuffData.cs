using ShootingGame.Data;
using UnityEngine;

public enum BuffType
{
    DamageBoost,
    SpeedBoost,
    Heal,
    ArmorBoost,
    CriticalChance,
    DodgeBoost,
    AttackSpeed,
    LifeSteal,
    PoisonImmunity,
    DamageReduction,
}

[CreateAssetMenu(fileName = "ItemBuffData", menuName = "Items/Single/ItemBuffData")]
public class ItemBuffData : ItemDataSO
{
    [SerializeField] private Sprite icon;
    [SerializeField] private BuffType buffType;
    [SerializeField] private int amount;
    [SerializeField] private float duration;
    public Sprite Icon => icon;
    public BuffType BuffType => buffType;
    public int Amount => amount;
    public float Duration => duration;
}

