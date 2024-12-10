using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text textTimer;

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

        while (TimeLeft > 0)
        {
            var second = totalTime % 60;
            var minute = (totalTime / 60) % 60;
            textTimer.text = minute.ToString() + ":" + second.ToString();
            yield return new WaitForSeconds(1f);
            TimeLeft--;
        }

        OnFinished?.Invoke();
    }
}
