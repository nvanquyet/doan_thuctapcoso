﻿using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; } 

    [SerializeField] private int currentCoin;
    public int CurrentCoin => currentCoin;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của CoinManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void AddCoin(int coin)
    {
        currentCoin += coin;
    }

    public void RemoveCoin(int coin)
    {
        currentCoin -= coin;
    }

    public int GetCoin()
    {
        return currentCoin;
    }

    public void ResetCoin()
    {
        currentCoin = 0;
    }
}
