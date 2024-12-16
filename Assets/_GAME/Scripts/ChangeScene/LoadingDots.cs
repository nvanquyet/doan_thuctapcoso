using UnityEngine;
using TMPro;

public class LoadingDots : MonoBehaviour
{
    public TMP_Text loadingText; 
    private string baseText = "LOADING"; 
    private float timer = 0f;
    private int dotCount = 0;
#if UNITY_EDITOR
    private void OnValidate()
    {
        loadingText = GetComponent<TMP_Text>();
    }
#endif
    void Update()
    {
        if(loadingText == null) return;
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            dotCount = (dotCount + 1) % 4; 
            loadingText.text = baseText + new string('.', dotCount); 
            timer = 0f; 
        }
    }
}
