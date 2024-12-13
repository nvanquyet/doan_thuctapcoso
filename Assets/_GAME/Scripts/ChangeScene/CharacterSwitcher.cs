using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitcher : MonoBehaviour
{
    public Transform characterParent;          
    public TMP_Text characterNameText;            
    public GameObject[] characterPrefabs;    
    public string[] characterNames;          
    private GameObject currentCharacter;
    public Sprite[] characterAvatars;
    public Image playerAvatarImage;
    private int currentIndex = 0;             

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % characterPrefabs.Length;
        SwitchCharacter();
    }

    public void PreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + characterPrefabs.Length) % characterPrefabs.Length;
        SwitchCharacter();
    }

    private void SwitchCharacter()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }


        currentCharacter = Instantiate(characterPrefabs[currentIndex], characterParent);
        currentCharacter.transform.localPosition = Vector3.zero;
        characterNameText.text = characterNames[currentIndex];
        if (characterAvatars != null && currentIndex < characterAvatars.Length)
        {
            playerAvatarImage.sprite = characterAvatars[currentIndex];
        }
    }
}
