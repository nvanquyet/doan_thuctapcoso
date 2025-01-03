using ShootingGame;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitcher : MonoBehaviour
{
    public Transform characterParent;          
    public TMP_Text characterNameText;
    //public Image playerAvatarImage;

    public Button nextButton;
    public Button preButton;

    public Animator animator;         

    private int index = 0;

    private void Start()
    {
        nextButton.onClick.AddListener(NextCharacter);
        preButton.onClick.AddListener(PreviousCharacter);
    }

    private void OnEnable()
    {
        SwitchCharacter();
    }
    public int CharacterIndex
    {
        get
        {
            var idx = UserData.CurrentCharacter + index;
            if (idx < 0) idx = GameData.Instance.Players.GetAllValue().Length - 1;
            if (idx >= GameData.Instance.Players.GetAllValue().Length) idx = 0;
            return idx;
        }
    }

    public void NextCharacter()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        index++;
        SwitchCharacter(true);
    }

    public void PreviousCharacter()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        index--;
        SwitchCharacter(false);
    }

    private void SwitchCharacter(bool increase = true)
    {
        var c = GameData.Instance.Players.GetValue(CharacterIndex);
        if(c.IsOwn || UserData.GetOwnerCharacter(CharacterIndex))
        {
            characterNameText.text = c.Appearance.Name;
            animator.runtimeAnimatorController = c.Animator;
        }
        else
        {
            if(increase) index++;
            else index--;
            if(index >= GameData.Instance.Players.GetAllValue().Length)
            {
                index = 0;
            }
            else if(index < 0)
            {
                index = GameData.Instance.Players.GetAllValue().Length - 1;
            }
            SwitchCharacter(increase);
            return;
        }
       
        //if (playerAvatarImage != null)
        //{
        //    playerAvatarImage.sprite = c.Appearance.Icon;
        //}
    }
}
