using ShootingGame;
using System;
using UnityEngine;

public class LevelProgesstion : MonoBehaviour
{
    [SerializeField] private ProgressBar bar;
    [SerializeField] private ParticleSystem levelUpEffect;
    public int Level { get; private set; } = 1;
    public int CurrentEXP { get; private set; } = 0;
    public int EXPToNextLevel => CalculateEXPForLevel(Level + 1);

    public bool IsLevelUp;

    public void Initialized()
    {
        Level = 1;
        CurrentEXP = 0;
        IsLevelUp = false;
        bar.UpdateProgess(CurrentEXP, EXPToNextLevel);
    }

    public void AddEXP(int exp)
    {
        CurrentEXP += exp;
        while (CurrentEXP >= EXPToNextLevel)
        {
            CurrentEXP -= EXPToNextLevel;
            LevelUp();
        }
        bar.UpdateProgess(CurrentEXP, EXPToNextLevel);
    }

    private int CalculateEXPForLevel(int level)
    {
        return 50 * level * level;
    }

    private void LevelUp()
    {
        Level++;
        IsLevelUp = true;
        levelUpEffect?.Play();
    }
}