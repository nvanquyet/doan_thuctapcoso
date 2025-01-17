using Mono.CSharp;
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
            return PlayerPrefs.GetInt("CurrentCoin", 0);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentCoin", value);
            EventDispatcher.Instance.Dispatch<GameEvent.CoinChange>();
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
            EventDispatcher.Instance.Dispatch<GameEvent.OnUserNameChanged>();
        }
    }

    public static (string, string) EmailPassword
    {
        get
        {
            return (PlayerPrefs.GetString("UserEmail", ""), PlayerPrefs.GetString("UserPassword", ""));
        }
        set
        {
            PlayerPrefs.SetString("UserEmail", value.Item1);
            PlayerPrefs.SetString("UserPassword", value.Item2);
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

    public static int CurrentEnergy
    {
        get
        {
            return PlayerPrefs.GetInt("Energy", GameConfig.Instance.MaxEnergy);
        }
        set
        {
            PlayerPrefs.SetInt("Energy", value >= 0 ? (value >= GameConfig.Instance.MaxEnergy ? GameConfig.Instance.MaxEnergy : value) : 0);
            EventDispatcher.Instance.Dispatch<GameEvent.EnergyChange>();
        }
    }

    public static void UseEnergy(DateTime lastTimeUpdate)
    {
        CurrentEnergy -= GameConfig.Instance.EnergyCost;
        Service.gI().UseEnergy(GameConfig.Instance.EnergyCost);
        //Set Time
        LastTimeUpdate = lastTimeUpdate;
        var e = GameConfig.Instance.MaxEnergy - CurrentEnergy;
        TimeFillMaxEnergy = new DateTime(LastTimeUpdate.Ticks).AddSeconds(e * GameConfig.Instance.EnergyGainInterval);
    }

    public static void SetTimeEnergy(long lastTimeUpdate)
    {
        DateTime timeUpdate;
        if(lastTimeUpdate <= 0) timeUpdate = DateTime.Now;
        else timeUpdate = DateTimeOffset.FromUnixTimeMilliseconds(lastTimeUpdate).UtcDateTime.ToLocalTime();
        SetTimeEnergy(timeUpdate);
    }
    public static void SetTimeEnergy(DateTime timeUpdate)
    {
        LastTimeUpdate = timeUpdate;
        var e = GameConfig.Instance.MaxEnergy - CurrentEnergy;
        TimeFillMaxEnergy = new DateTime(LastTimeUpdate.Ticks).AddSeconds(e * GameConfig.Instance.EnergyGainInterval);
    }

    public static DateTime LastTimeUpdate
    {
        get
        {
            return DateTime.Parse(PlayerPrefs.GetString("LastTimePlayed", DateTime.Now.ToString()));
        }
        set
        {
            PlayerPrefs.SetString("LastTimePlayed", value.ToString("o"));
        }
    }

    public static DateTime TimeFillMaxEnergy
    {
        get
        {
            return DateTime.Parse(PlayerPrefs.GetString("TimeFillMaxEnergy", DateTime.Now.ToString()));
        }
        set
        {
            PlayerPrefs.SetString("TimeFillMaxEnergy", value.ToString("o"));
        }
    }

    public static void SetGunProjectile(int idGun, int index)
    {
        PlayerPrefs.SetInt($"GunProjectile_{idGun}", Mathf.Max(0, index));
    }

    public static int GetGunProjectile(int idGun)
    {
        return PlayerPrefs.GetInt($"GunProjectile_{idGun}", -1);
    }

    public static void LoadPlayerData(Server.Player player, string[] characterOwn)
    {
        GameService.LogColor($"Load AllData");
        CurrentCoin = (int) player.coin;
        UserName = player.name;
        GameConfig.Instance.MaxEnergy = player.maxEnergy;
        CurrentEnergy = player.energy;
        SetTimeEnergy(player.lastUpdateEnergy);
        for (int i = 0; i < characterOwn.Length; i++)
        {
            var id = Int32.Parse(characterOwn[i]);
            SetOwnerCharacter(id);
        }
    }

    public static void Logout()
    {
        IsLogin = false;
        PlayerPrefs.DeleteAll();
        Service.gI().Logout();
    }

}
