using DG.Tweening;
using ShootingGame;
using UnityEngine;

public class FloatingTextItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textMesh;
#if UNITY_EDITOR
    private void OnValidate()
    {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
    }

#endif
    public void Initialize(string text, Color color, ObjectPooling<FloatingTextItem> pool)
    {
        textMesh.text = text;
        textMesh.color = color;
        if (color == Color.red) textMesh.fontSize *= 1.5f;
        this.transform.DOMoveY(this.transform.position.y + 1, 0.5f);
        this.textMesh.DOFade(1, 0.2f).From(0).OnComplete(() =>
        {
            this.textMesh.DOFade(0, 0.2f).OnComplete(() =>
            {
                pool.Recycle(this);
            }).SetDelay(0.1f);
        });
    }
}
