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
    [SerializeField] private int timeFirstLoading = 3;
    private bool isFirstLoading = true;

    private void Start()
    {
        isFirstLoading = true;
    }

    public void LoadSceneAsync(string sceneName, Action<float> onProgess = null, System.Action onSuccess = null, Action onFailed = null)
    {
        var buildIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
        LoadSceneAsync(buildIndex, onProgess, onSuccess, onFailed);
    }



    public void LoadSceneAsync(int buildIndex, Action<float> onProgess = null, System.Action onSuccess = null, Action onFailed = null)
    {
        Action callBack = () =>
        {
            Hide(group != null, () => isFirstLoading = false);
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
            group.DOFade(0, isFirstLoading ? 1f : animTime).SetUpdate(true).OnComplete(() =>
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
        if (!isFirstLoading)
        {
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
        }
        else
        {
            float progess = 0f;
            while (progess <= timeFirstLoading)
            {
                progess += Time.deltaTime;
                onProgess?.Invoke(progess / timeFirstLoading);
                if (bar != null) bar.UpdateProgess(progess / timeFirstLoading);
                yield return null;
            }
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    break;
                }
                yield return null;
            }
            asyncOperation.allowSceneActivation = true;
            onSuccess?.Invoke();
            yield break;
        }

        onFailed?.Invoke();
    }
}
