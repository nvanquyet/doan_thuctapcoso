using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShootingGame;

public class MoneyBar : Frame
{
    public Image moneyBarFill;            
    public TextMeshProUGUI moneyBarText;  
    public float maxMoney = 1000;         
    private float currentMoney;           

    void Start()
    {
        SetMoney(0); 
    }
    public void AddMoney(float amount)
    {
        currentMoney += amount;
        if (currentMoney > maxMoney) currentMoney = maxMoney; 
        UpdateMoneyBar();
    }

    public void SubtractMoney(float amount)
    {
        currentMoney -= amount;
        if (currentMoney < 0) currentMoney = 0; 
        UpdateMoneyBar();
    }

    private void UpdateMoneyBar()
    {
        float fillAmount = currentMoney / maxMoney; 
        moneyBarFill.fillAmount = fillAmount;      
        moneyBarText.text = $"{currentMoney:0}/{maxMoney:0}"; 
    }
    public void SetMoney(float amount)
    {
        currentMoney = Mathf.Clamp(amount, 0, maxMoney);
        UpdateMoneyBar();
    }
}
