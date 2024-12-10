using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShootingGame;

public class LoadingDots : Frame
{
    public TMP_Text loadingText; 
    private string baseText = "LOADING"; 
    private float timer = 0f;
    private int dotCount = 0;

    void Start()
    {
        loadingText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        timer += Time.deltaTime;

        
        if (timer >= 0.5f)
        {
            dotCount = (dotCount + 1) % 4; 
            loadingText.text = baseText + new string('.', dotCount); 
            timer = 0f; 
        }
    }
}
