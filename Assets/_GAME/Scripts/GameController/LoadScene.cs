using DG.Tweening;
using ShootingGame;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : Frame
{
    [SerializeField] private ProgressBar bar;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private int timeFirstLoading = 2;

    public void Initialized()
    {
        Action callBack = () =>
        {
            Hide(group != null);
        };
        StartCoroutine(IELoadAsyncCoroutine(timeFirstLoading, callBack));
    }

    public void LoadDirectScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }


    public void LoadSceneAsync(int buildIndex, Action<float> onProgess = null, System.Action onSuccess = null, Action onFailed = null)
    {
        Action callBack = () =>
        {
            Hide(group != null);
        };
        onSuccess += callBack;
        onFailed += callBack;

        UIPopUpCtrl.Instance.Show<LoadScene>(false, () =>
        {
            StartCoroutine(IELoadSceneAsyncCoroutine(buildIndex, onProgess, onSuccess, onFailed));
        });
    }


    public override void Hide(bool animate = true, Action callback = null)
    {
        if (group && animate)
        {
            if (!gameObject.activeSelf) return;
            group.DOKill();
            group.DOFade(0, animTime).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
                if (callback != null) callback.Invoke();
            }).From(1);
        }
        else
        {
            base.Hide(animate, callback);
        }

    }

    private IEnumerator IELoadSceneAsyncCoroutine(int buildIndex, Action<float> onProgess, Action onSuccess, Action onFailed)
    {
        // Load the scene asynchronously
        var asyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        asyncOperation.allowSceneActivation = false;
        bar?.gameObject.SetActive(true);
        bar?.UpdateProgess(0);
        // Wait until the asynchronous scene fully loads
        while (!asyncOperation.isDone)
        {
            onProgess?.Invoke(asyncOperation.progress);
            if (bar != null) bar.UpdateProgess(asyncOperation.progress);
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                onSuccess?.Invoke();
                yield break;
            }
            yield return null;
        }
        onFailed?.Invoke();
    }

    private IEnumerator IELoadAsyncCoroutine(int timeLoad, Action onSuccess)
    {
        float time = 0f;
        bar?.gameObject.SetActive(true);
        bar?.UpdateProgess(time);
        while (time < timeLoad)
        {
            time += Time.deltaTime;
            if (bar != null) bar.UpdateProgess(time/timeLoad);
            yield return null;
        }
        onSuccess?.Invoke();
    }
}
