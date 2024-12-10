using ShootingGame;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyProperties", menuName = "_GAME/Enemy Properties Data")]
public class EnemyPropertiesData : ScriptableObject
{
    [SerializeField] private short baseHealth;
    [SerializeField] private short baseDamage;
    [SerializeField] private short baseSpeed;
    [SerializeField] private int baseEXP;
    [SerializeField] private float growthRate = 1.2f;


    public short BaseHealth => baseHealth;
    public short BaseDamage => baseDamage;
    public short BaseSpeed => baseSpeed;
    public int BaseEXP => baseEXP;
    public float GrowthRate => growthRate;

    public override string ToString()
    {
        return $"Base Health: {baseHealth} - Base Damage: {baseDamage} - Base Speed: {baseSpeed} - Base EXP: {baseEXP} - Growth Rate: {growthRate}";
    }
}