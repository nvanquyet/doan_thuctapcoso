using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using ShootingGame;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textTimer;

    private Coroutine coutdownRoutine;

    public int TimeLeft { get; private set; }

    public void SetTimer(int time, Action OnCompleted = null)
    {
        if (coutdownRoutine != null)
        {
            StopCoroutine(coutdownRoutine);
            coutdownRoutine = null;
        }
        coutdownRoutine = StartCoroutine(IEStartTimer(time, OnCompleted));
    }

    public void StopTimer()
    {
        if (coutdownRoutine != null)
        {
            StopCoroutine(coutdownRoutine);
            coutdownRoutine = null;
        }
    }

    private IEnumerator IEStartTimer(int totalTime, Action OnFinished = null)
    {
        TimeLeft = totalTime;
        textTimer.color = Color.white;
        textTimer.gameObject.SetActive(true);
        while (TimeLeft >= 0)
        {
            var second = TimeLeft % 60;
            var minute = (TimeLeft / 60) % 60;
            textTimer.text = $"{GetString(minute)}:{GetString(second)}";
            if (second < 10)
            {
                textTimer.color = second % 2 == 0 ? Color.white : Color.red;
            }
            TimeLeft--;
            yield return new WaitForSeconds(1f);
        }

        OnFinished?.Invoke();
        textTimer.gameObject.SetActive(false);
    }

    public string GetString(int value)
    {
        if(value < 10 && value >= 0) return "0" + value.ToString();
        return value.ToString();
    }
}
