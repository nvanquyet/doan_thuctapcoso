using ShootingGame;
using System.Collections.Concurrent;
using UnityEngine;

public class UnityMainThreadDispatcher : SingletonBehaviourDontDestroy<UnityMainThreadDispatcher>
{
    private static readonly ConcurrentQueue<System.Action> _actions = new ConcurrentQueue<System.Action>();

    public static void Enqueue(System.Action action = null)
    {
        if(action != null) _actions.Enqueue(action);
    }


    private void Update()
    {
        while (_actions.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }
}
