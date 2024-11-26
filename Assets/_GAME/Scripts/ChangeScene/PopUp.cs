using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class PopUp : MonoBehaviour
{
    public TMP_Text popUpNameText;
    public void SetPopUpName(string text)
    {
        popUpNameText.text = text;
    }
}
