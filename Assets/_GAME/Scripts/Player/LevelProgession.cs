using ShootingGame;
using System;
using UnityEngine;

public class LevelProgesstion : MonoBehaviour
{
    [SerializeField] private ProgressBar bar;
    public int Level { get; private set; } = 1;
    public int CurrentEXP { get; private set; } = 0;
    public int EXPToNextLevel => CalculateEXPForLevel(Level + 1);

    public Action OnLevelUp;

    public void Initialized(Action OnLevelUp = null)
    {
        Level = 1;
        CurrentEXP = 0;
        this.OnLevelUp = OnLevelUp;
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
        return 100 * level * level;
    }

    private void LevelUp()
    {
        Level++;
        OnLevelUp?.Invoke();
    }
}