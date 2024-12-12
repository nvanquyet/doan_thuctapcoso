using UnityEngine;
[CreateAssetMenu(fileName = "EnemyProperties", menuName = "_GAME/Enemy Properties Data")]
public class EnemyPropertiesData : ScriptableObject
{
    [SerializeField] private short baseHealth;
    [SerializeField] private short baseDamage;
    [SerializeField] private short baseSpeed;
    [SerializeField] private int baseEXP;
    [SerializeField] private int baseCoin;
    [SerializeField] private float growthRate = 1.2f;


    public short BaseHealth => baseHealth;
    public short BaseDamage => baseDamage;
    public short BaseSpeed => baseSpeed;
    public int BaseEXP => baseEXP;
    public int BaseCoin => baseCoin;
    public float GrowthRate => growthRate;
}