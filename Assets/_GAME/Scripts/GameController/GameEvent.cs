using System.Collections.Generic;
using Mono.CSharp;
using ShootingGame;
using ShootingGame.Data;
using UnityEngine;

public sealed class GameEvent
{

    public struct OnWaveClear : IEventParam
    {
        public int wave;
    }

    public struct OnNextWave : IEventParam
    {
        public Player player;

        public List<ItemEquiqmentData> allEquiqments;
        public List<ItemWeaponData> allWeapons;
        public List<ItemBuffData> allBuffItems;
    }

    public struct OnPlayerDead : IEventParam { }

    public struct OnEndGame : IEventParam
    {
        public bool isWin;
        public int enemiesDefeated;
        public int totalEnemies;
        public float timeLeft;
    }

    public struct OnShowFloatingText : IEventParam
    {
        public string text;
        public Vector3 worldPos;
        public Color color;
    }

    public struct OnLevelUp : IEventParam
    {
        public Player player;
    }

}
