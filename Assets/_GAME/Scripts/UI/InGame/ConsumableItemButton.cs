using DG.Tweening;
using ShootingGame;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public interface IBooster
{
    void Use();
}

public class ConsumableItemButton : MonoBehaviour, IBooster
{
    [SerializeField] private Image mask, icon;
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI countText;
    private float duration;
    private int amount;
    private ItemBuffData data;
    private Player target;

    public void Initialized(ItemBuffData data, Player target)
    {
        this.data = data;

        this.icon.sprite = data.Appearance.Icon;
        this.mask.sprite = data.Appearance.Icon;
        this.amount = data.Amount;
        this.target = target;

        countText.text = amount.ToString();
        btn.onClick.AddListener(Use);
    }

    public virtual void Use()
    {
        this.amount--;
        countText.text = amount.ToString();
        target.UseItemBuff(this.data);
        StartCoroutine(IECountDownButton(5));
    }


    IEnumerator IECountDownButton(float duration)
    {
        btn.interactable = false;
        var timeCountDown = 0f;
        icon.fillAmount = 0;
        while (timeCountDown <= duration)
        {
            timeCountDown += Time.deltaTime;
            icon.fillAmount = timeCountDown / duration;
            yield return null;
        }
        btn.interactable = this.amount > 0;
    }
}
