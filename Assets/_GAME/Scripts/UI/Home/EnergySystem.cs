using ShootingGame;
using System;
using System.Collections;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    [SerializeField] private ProgressBar slider;
    [SerializeField] private TMPro.TextMeshProUGUI txtTime;
    public bool HasEnergy => UserData.CurrentEnergy > 0;
    private void OnEnable()
    {
        StartCoroutine(UpdateEnergy(true));
    }

    private IEnumerator UpdateEnergy(bool checkEnergyGain = false)
    {

        // Get the last time the player played the game
        DateTime lastTime = UserData.LastTimePlayed;
        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Calculate the time difference between the last time the player played the game and the current time
        TimeSpan timeDifference = currentTime - lastTime;

        if(checkEnergyGain) CheckEnergyStatus(timeDifference);

        // If the player has reached the maximum energy
        if (UserData.CurrentEnergy >= GameConfig.Instance.MaxEnergy)
        {
            // Hide the time text
            txtTime.gameObject.SetActive(false);
            yield break;
        }
        // Show the time text
        txtTime.gameObject.SetActive(true);

        // Calculate the time until the player gains the next energy
        TimeSpan timeUntilNextEnergy = TimeSpan.FromSeconds(GameConfig.Instance.EnergyGainInterval - timeDifference.TotalSeconds % GameConfig.Instance.EnergyGainInterval);

        //Using while loop to update the time text every second

        while (timeUntilNextEnergy.TotalSeconds > 0)
        {
            // Update the time text
            txtTime.text = $"{timeUntilNextEnergy.Hours:D2}:{timeUntilNextEnergy.Minutes:D2}:{timeUntilNextEnergy.Seconds:D2}";

            // Wait for 1 second
            yield return new WaitForSeconds(1);

            // Update the time until the player gains the next energy
            timeUntilNextEnergy = TimeSpan.FromSeconds(GameConfig.Instance.EnergyGainInterval - (DateTime.Now - UserData.LastTimePlayed).TotalSeconds % GameConfig.Instance.EnergyGainInterval);
        }
    }

    public bool UsingEnergy()
    {
        // If the player has energy
        if (UserData.CurrentEnergy > 0)
        {
            // Decrease the player's energy
            UserData.CurrentEnergy--;

            // Set the last time the player played the game to the current time
            if (UserData.CurrentEnergy == GameConfig.Instance.MaxEnergy - 1)
            {
                UserData.LastTimePlayed = DateTime.Now;
                StartCoroutine(UpdateEnergy());
            }

            UpdateSlider();

            // Return true to indicate that the player has used energy
            return true;
        }
        return false;
    }

    private void UpdateSlider()
    {
        // Update the energy slider
        slider.UpdateProgess(UserData.CurrentEnergy * 1.0f / GameConfig.Instance.MaxEnergy);
        // Update the energy text
    }

    private void CheckEnergyStatus(TimeSpan timeDifference)
    {
        // Calculate the energy the player should have gained
        int energyGained = (int)(timeDifference.TotalSeconds / GameConfig.Instance.EnergyGainInterval);

        // If the player has gained energy
        if (energyGained > 0) UserData.CurrentEnergy += energyGained;
        UpdateSlider();
    }
}
