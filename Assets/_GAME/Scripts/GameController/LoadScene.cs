using ShootingGame;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : Frame
{
    [SerializeField] private ProgressBar bar;

    public void LoadSceneAsync(string sceneName, Action<float> onProgess = null, System.Action onSuccess = null, Action onFailed = null)
    {
        var buildIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
        LoadSceneAsync(buildIndex, onProgess, onSuccess, onFailed);
    }

    public void LoadSceneAsync(int buildIndex, Action<float> onProgess = null, System.Action onSuccess = null, Action onFailed = null)
    {
        onSuccess += () => Hide(false);
        onFailed += () => Hide(false);

        UIPopUpCtrl.Instance.Show<LoadScene>(false, () =>
        {
            StartCoroutine(IELoadSceneAsyncCoroutine(buildIndex, onProgess, onSuccess, onFailed));
        });
    }

    private IEnumerator IELoadSceneAsyncCoroutine(int buildIndex, Action<float> onProgess, Action onSuccess, Action onFailed)
    {
        // Load the scene asynchronously
        var asyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        asyncOperation.allowSceneActivation = false;
        if(bar != null)
        {
            bar.gameObject.SetActive(true);
            bar.UpdateProgess(0);
        }
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
}
