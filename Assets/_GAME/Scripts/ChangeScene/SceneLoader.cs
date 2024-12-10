using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ShootingGame;

public class SceneLoader : Frame
{
    public GameObject loaderUI;
    public Slider progressSlider;
    public float sliderSpeed = 0.05f;
    public Image loadingIcon;
    public float rotationSpeed = 200f;
    public Sprite[] loadingSprites;

    public void LoadScene(int index)
    {
        RandomizeLoadingIcon();
        StartCoroutine(IELoadScene(index));
        
    }
    /*public IEnumerator LoadScene_Coroutine(int index)
    {
        progressSlider.value = 0;
        loaderUI.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime * sliderSpeed);
            progressSlider.value = progress;
            if(progress >= 0.1f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }*/

    private void RandomizeLoadingIcon()
    {
        if (loadingSprites.Length > 0)
        {
            Sprite randomSprite = loadingSprites[Random.Range(0, loadingSprites.Length)];
            loadingIcon.sprite = randomSprite;
        }
    }

    public IEnumerator IELoadScene(int index)
    {
        progressSlider.value = 0;
        loaderUI.SetActive(true);
        StartCoroutine(AnimateLoadingIcon());
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;

        float elapsedTime = 0f; 
        float duration = 10f;   

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration); 
            progressSlider.value = progress;

            
            if (progress >= 1f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
    private IEnumerator AnimateLoadingIcon()
    {
        while (loaderUI.activeSelf) 
        {
            loadingIcon.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
