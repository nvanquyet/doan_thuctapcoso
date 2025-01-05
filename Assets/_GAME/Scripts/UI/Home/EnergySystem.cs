using ShootingGame;
using System;
using System.Collections;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    [SerializeField] private ProgressBar slider;
    [SerializeField] private TMPro.TextMeshProUGUI txtTime;
    public bool HasEnergy => UserData.CurrentEnergy > 0;
    private Coroutine updateEnergyCoroutine;
    private void OnEnable()
    {
        CheckEnergy();
    }
    private void Start()
    {
        this.AddListener<GameEvent.OnLoadPlayer>(OnLoadPlayer, false);
    }

    private void OnLoadPlayer(GameEvent.OnLoadPlayer param)
    {
        if(updateEnergyCoroutine != null) StopCoroutine(updateEnergyCoroutine);
        CheckEnergy();
    }

    private void CheckEnergy()
    {
        updateEnergyCoroutine = StartCoroutine(UpdateEnergy(true));
    }

    private IEnumerator UpdateEnergy(bool checkEnergyGain = false)
    {
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
        TimeSpan timeUntilFullEnergy = UserData.TimeFillMaxEnergy - DateTime.Now;
        UpdateSlider();
        //Using while loop to update the time text every second
        while (timeUntilFullEnergy.TotalSeconds > 0)
        {
            // Update the time text
            txtTime.text = $"{timeUntilFullEnergy.Hours:D2}:{timeUntilFullEnergy.Minutes:D2}:{timeUntilFullEnergy.Seconds:D2}";
            // Wait for 1 second
            yield return new WaitForSeconds(1);
            UpdateSlider();
            // Update the time difference
            timeUntilFullEnergy -= TimeSpan.FromSeconds(1);
        }
    }

    public bool UsingEnergy()
    {
        // If the player has energy
        if (UserData.CurrentEnergy > 0)
        {
            // Decrease the player's energy
            UserData.UseEnergy(DateTime.Now);
            StartCoroutine(UpdateEnergy());
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
