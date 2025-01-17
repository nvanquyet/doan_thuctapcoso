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


    private void Awake()
    {
        this.AddListener<GameEvent.OnLoadPlayer>(OnLoadPlayer, false);
    }
    private void OnEnable()
    {
        Invoke(nameof(CheckEnergy),.5f);
    }
    private void OnLoadPlayer(GameEvent.OnLoadPlayer param) => CheckEnergy();

    private void CheckEnergy()
    {

        if (updateEnergyCoroutine != null)
        {
            StopCoroutine(updateEnergyCoroutine);
        }

        updateEnergyCoroutine = StartCoroutine(UpdateEnergy(true));
    }

    private IEnumerator UpdateEnergy(bool checkEnergyGain = false)
    { 
        if (UserData.CurrentEnergy >= GameConfig.Instance.MaxEnergy)
        {
            txtTime.gameObject.SetActive(false);
            UpdateSlider();
            yield break;
        }

        txtTime.gameObject.SetActive(true);
        UpdateSlider();
        while (UserData.CurrentEnergy < GameConfig.Instance.MaxEnergy)
        {
            TimeSpan timeUntilFullEnergy = UserData.TimeFillMaxEnergy - DateTime.Now;
            if (timeUntilFullEnergy.TotalSeconds <= 0)
            {
                txtTime.gameObject.SetActive(false);
                UpdateSlider();
                yield break;
            }
            txtTime.text = $"{timeUntilFullEnergy.Hours:D2}:{timeUntilFullEnergy.Minutes:D2}:{timeUntilFullEnergy.Seconds:D2}";
            
            yield return new WaitForSeconds(1);
        }
    }

    public bool UsingEnergy()
    {
        if (UserData.CurrentEnergy > 0)
        {
            UserData.UseEnergy(DateTime.Now);

            UserData.TimeFillMaxEnergy = DateTime.Now.AddSeconds(GameConfig.Instance.EnergyGainInterval);

            if (updateEnergyCoroutine != null)
            {
                StopCoroutine(updateEnergyCoroutine);
            }
            updateEnergyCoroutine = StartCoroutine(UpdateEnergy());

            UpdateSlider();

            return true;
        }
        return false;
    }

    private void UpdateSlider()
    {
        slider.UpdateProgess(UserData.CurrentEnergy * 1.0f / GameConfig.Instance.MaxEnergy);
    }

}
