using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ShootingGame;
public class PopUp : Frame
{
    public TMP_Text popUpNameText;
    public void SetPopUpName(string text)
    {
        popUpNameText.text = text;
    }
}
