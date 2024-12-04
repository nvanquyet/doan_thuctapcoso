using ShootingGame;
using UnityEngine;

public class FloatingTextPopUp : Frame
{
    [SerializeField] private FloatingTextItem prefabs;
    [SerializeField] private int amountPooling = 10;
    private ObjectPooling<FloatingTextItem> objectPooling;
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (prefabs == null) prefabs = GetComponentInChildren<FloatingTextItem>();
        if(prefabs != null) prefabs.gameObject.SetActive(false);
    }

#endif
    private void Start()
    {
        objectPooling = new ObjectPooling<FloatingTextItem>(prefabs, amountPooling, MainFrame);
        this.AddListener<GameEvent.OnShowFloatingText>(OnShowFloatingText, false);
    }

    private void OnShowFloatingText(GameEvent.OnShowFloatingText param)
    {
        var floatingText = objectPooling.Get();
        floatingText.transform.position = param.worldPos;
        floatingText.Initialize(param.text, param.color, objectPooling);
    }
}
