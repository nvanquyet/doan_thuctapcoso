using ShootingGame;
using System;
using UnityEngine;

public partial class UserData
{
    public static int CurrentCharacter
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentCharacter", 0);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentCharacter", value);
        }
    }

    public static int CurrentCoin
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentCoin", 120);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentCoin", value);
        }
    }

    public static string UserName
    {
        get
        {
            return PlayerPrefs.GetString("UserName", "UserName");
        }
        set
        {
            PlayerPrefs.SetString("UserName", value);
        }
    }

    public static bool IsLogin
    {
        get
        {
            return PlayerPrefs.GetInt("IsLogin", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("IsLogin", value ? 1 : 0);
        }
    }

    public static int BestScore
    {
        get
        {
            return PlayerPrefs.GetInt("BestScore", 0);
        }
        set
        {
            PlayerPrefs.SetInt("BestScore", value);
        }
    }

    public static int Coin
    {
        get
        {
            return PlayerPrefs.GetInt("Coin", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Coin", value);
        }
    }

    public static int CurrentEnergy
    {
        get
        {
            return PlayerPrefs.GetInt("Energy", GameConfig.Instance.MaxEnergy);
        }
        set
        {
            PlayerPrefs.SetInt("Energy", value >= 0 ? (value >= GameConfig.Instance.MaxEnergy ? GameConfig.Instance.MaxEnergy : value) : 0);
        }
    }

    public static DateTime LastTimePlayed
    {
        get
        {
            return DateTime.Parse(PlayerPrefs.GetString("LastTimePlayed", DateTime.Now.ToString()));
        }
        set
        {
            PlayerPrefs.SetString("LastTimePlayed", value.ToString());
        }
    }
}
