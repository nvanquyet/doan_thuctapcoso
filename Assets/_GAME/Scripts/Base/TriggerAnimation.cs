using ShootingGame;
using System;
using UnityEngine;

public interface IAnimTrigger
{
    void TriggerAnimaiton(string nameAnimation);
}
public class TriggerAnimation : MonoBehaviour, IAnimTrigger
{
    public Action<string> OnTriggerAction;
    public void TriggerAnimaiton(string nameAnimation)
    {
        OnTriggerAction?.Invoke(nameAnimation);
    }
}
