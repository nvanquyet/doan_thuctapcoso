using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ShootingGame.Data;

public class RandomItemSelector : MonoBehaviour
{
    public WeaponData weaponData;   // Reference to the ScriptableObject containing weapon data list
    public WeaponData weaponData1;
    public WeaponData weaponData2;

    public Image image1;            // Image UI to display the icon of item 1
    public Image image2;            // Image UI to display the icon of item 2
    public Image image3;            // Image UI to display the icon of item 3

    public Text text1;              // Text UI to display description of item 1
    public Text text2;              // Text UI to display description of item 2
    public Text text3;              // Text UI to display description of item 3

    // Player level
    public int playerLevel;

    // Item spawn rates for each level
    public int spawnRate1Lv1;
    public int spawnRate2Lv1;
    public int spawnRate3Lv1;

    public int spawnRate1Lv2;
    public int spawnRate2Lv2;
    public int spawnRate3Lv2;

    public int spawnRate1Lv3;
    public int spawnRate2Lv3;
    public int spawnRate3Lv3;

    // Method to roll 3 random items and display their icons and descriptions
    public void RollRandomItems()
    {
        int spawnRate1 = 0;
        int spawnRate2 = 0;
        int spawnRate3 = 0;

        // Set spawn rates based on the player's level
        if (playerLevel >= 1 && playerLevel < 5)
        {
            spawnRate1 = spawnRate1Lv1;
            spawnRate2 = spawnRate2Lv1;
            spawnRate3 = spawnRate3Lv1;
        }
        else if (playerLevel >= 5 && playerLevel < 10)
        {
            spawnRate1 = spawnRate1Lv2;
            spawnRate2 = spawnRate2Lv2;
            spawnRate3 = spawnRate3Lv2;
        }
        else if (playerLevel >= 10)
        {
            spawnRate1 = spawnRate1Lv3;
            spawnRate2 = spawnRate2Lv3;
            spawnRate3 = spawnRate3Lv3;
        }

        List<WeaponData> selectedWeapons = new List<WeaponData>();

        // Instantiate a random generator
        System.Random random = new System.Random();
        while (true)
        {
            int randomValue = random.Next(0, 100);  // Generate a single random value between 0 and 99

            // Check ranges based on cumulative probabilities
            if (randomValue < spawnRate1)
            {
                selectedWeapons.Add(weaponData);  // 30% chance
            }
            else if (randomValue < spawnRate1 + spawnRate2 && randomValue > spawnRate1)
            {
                selectedWeapons.Add(weaponData1);  // 20% chance (cumulative: 30% + 20% = 50%)
            }
            else if (randomValue < 100 && randomValue > spawnRate1 + spawnRate2)
            {
                selectedWeapons.Add(weaponData2);  // 50% chance (cumulative: 30% + 20% + 50% = 100%)
            }

            if (selectedWeapons.Count == 3)
            {
                break;
            }
        }


        if (selectedWeapons.Count >= 3)
        {
            // Select three unique WeaponData entries for display
            List<int> selectedIndices = new List<int>();
            while (selectedIndices.Count < 3)
            {
                int randomIndex = Random.Range(0, selectedWeapons.Count);
                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                }
            }

            // Update UI with selected weapon data
            UpdateUI(selectedWeapons[selectedIndices[0]], image1, text1);
            UpdateUI(selectedWeapons[selectedIndices[1]], image2, text2);
            UpdateUI(selectedWeapons[selectedIndices[2]], image3, text3);
        }
        else
        {
            Debug.LogWarning("Not enough data to display 3 items");
        }
    }

    // Helper method to update the UI elements with weapon data
    private void UpdateUI(WeaponData weaponData, Image image, Text text)
    {
        if (weaponData != null && weaponData.DataDict.Count > 0)
        {
            // Assuming DataDict uses integer keys, get a random weapon from DataDict
            List<int> keys = new List<int>(weaponData.DataDict.Keys);
            int randomKey = keys[Random.Range(0, keys.Count)];

            WeaponVisualStruct weaponVisual = weaponData.DataDict[randomKey].VisualAttribute.GetVisual<WeaponVisualStruct>();

            // Set icon and description for the UI
            image.sprite = weaponVisual.Icon;
            text.text = weaponVisual.Description;
        }
        else
        {
            Debug.LogWarning("WeaponData or DataDict is missing data.");
        }
    }
}