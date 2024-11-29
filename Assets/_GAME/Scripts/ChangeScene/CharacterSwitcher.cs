using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSwitcher : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public Image characterImage;
    public Sprite[] characterSprites;
    public string[] characterNames;
    private int currentIndex = 0;      

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % characterSprites.Length; 
        UpdateCharacterImage();
    }

    public void PreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + characterSprites.Length) % characterSprites.Length;
        UpdateCharacterImage();
    }

    private void UpdateCharacterImage()
    {
        characterImage.sprite = characterSprites[currentIndex];
        characterNameText.text = characterNames[currentIndex];
    }
}
