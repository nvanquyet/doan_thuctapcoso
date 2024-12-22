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
            var idx = Mathf.Clamp(UserData.CurrentCharacter + index, 0, GameData.Instance.Players.GetAllValue().Length - 1);
            if (idx <= 0)
            {
                preButton.interactable = false;
                nextButton.interactable = true;
            }
            else if (idx >= GameData.Instance.Players.GetAllValue().Length - 1)
            {
                nextButton.interactable = false;
                preButton.interactable = true;
            }else
            {
                nextButton.interactable = true;
                preButton.interactable = true;
            }
            return idx;
        }
    }

    public void NextCharacter()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        index++;
        SwitchCharacter();
    }

    public void PreviousCharacter()
    {
        SFX.Instance.PlaySound(AudioEvent.ButtonClick);
        index--;
        SwitchCharacter();
    }

    private void SwitchCharacter()
    {
        var c = GameData.Instance.Players.GetValue(CharacterIndex);
        characterNameText.text = c.Appearance.Name;
        animator.runtimeAnimatorController = c.Animator;
        //if (playerAvatarImage != null)
        //{
        //    playerAvatarImage.sprite = c.Appearance.Icon;
        //}
    }
}
