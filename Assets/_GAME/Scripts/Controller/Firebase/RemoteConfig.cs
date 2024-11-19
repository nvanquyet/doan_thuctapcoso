using UnityEngine;
using Firebase;
using Firebase.RemoteConfig;
using ShootingGame;
using System.Threading.Tasks;
using System;
using Firebase.Extensions;
public class RemoteConfig : SingletonBehaviourDontDestroy<RemoteConfig>
{
    private bool isInitialized = false;
    protected override void OnAwake()
    {
        base.OnAwake();
        FetchDataAsync();
    }

    [ContextMenu("Test Remote Config")]
    public void TestRemoteconfig()
    {
        if(!isInitialized)
        {
            Debug.LogError("Remote Config is not initialized yet.");
            return;
        }
        CheckAndInitializeFirebaseAsync(() => Debug.Log("Value for myKey: " + FirebaseRemoteConfig.DefaultInstance.GetValue("LevelSpawnRadius").DoubleValue));
    }

    #region  Initialize
    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task =>
            {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                isInitialized = true;
            });
    }
    #endregion
    private async void CheckAndInitializeFirebaseAsync(Action actionComplete = null)
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            Debug.Log("Firebase is ready and initialized.");
            actionComplete?.Invoke();
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    }
}
