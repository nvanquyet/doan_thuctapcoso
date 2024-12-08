using UnityEngine;
using TMPro;
using System.Collections;
using ShootingGame;

public class LoadingHints : Frame
{
    public TMP_Text hintText; 
    public string[] hints; 
    public float fadeDuration = 0.01f;

    private Coroutine hintCoroutine;
    private int lastHintIndex = -1; 

    void Start()
    {
        hintText = GetComponent<TMP_Text>();
        if (hints.Length > 0)
        {
            ShowHintImmediately(GetRandomHintIndex());
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (hintCoroutine != null)
            {
                StopCoroutine(hintCoroutine);
            }
            hintCoroutine = StartCoroutine(ShowHintWithEffect(GetRandomHintIndex()));
        }
    }

    void ShowHintImmediately(int index)
    {
        hintText.text = hints[index];
        hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, 1f);
    }

    IEnumerator ShowHintWithEffect(int index)
    {
        yield return FadeText(0);
        hintText.text = hints[index];
        yield return FadeText(1);

        hintCoroutine = null; 
    }

    IEnumerator FadeText(float targetAlpha)
    {
        float startAlpha = hintText.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, newAlpha);
            yield return null;
        }

        hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, targetAlpha);
    }

    int GetRandomHintIndex()
    {
        if (hints.Length <= 1) return 0; 

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, hints.Length);
        } while (randomIndex == lastHintIndex); 

        lastHintIndex = randomIndex; 
        return randomIndex;
    }
}
