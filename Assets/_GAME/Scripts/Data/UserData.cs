using System.Collections;
using System.Collections.Generic;
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
}
