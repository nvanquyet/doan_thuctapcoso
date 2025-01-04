using ShootingGame;
using System;
using System.Collections;
using UnityEngine;

public class ServerDataLoader : SingletonBehaviourDontDestroy<ServerDataLoader>
{
    protected override void OnAwake()
    {
        base.OnAwake();
        //Start connect to server
        StartCoroutine(IEConnectToServer());
    }

    private string IEConnectToServer()
    {
        throw new NotImplementedException();
    }
}
