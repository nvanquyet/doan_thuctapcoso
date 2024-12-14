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
    [SerializeField] private BuffType buffType;
    [SerializeField] private int amount;
    [SerializeField] private float duration;

    public BuffType BuffType => buffType;
    public int Amount => amount;
    public float Duration => duration;
}

