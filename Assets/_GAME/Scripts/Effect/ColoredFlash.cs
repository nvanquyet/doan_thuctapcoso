using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredFlash : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    [SerializeField] SpriteRenderer spriteRenderer;

    private Material originalMaterial;
    private Coroutine flashRoutine;

    public void SetSpriteRenderer(SpriteRenderer spriteRenderer) {
        this.spriteRenderer = spriteRenderer;
        InitMaterial();
    }

    void Start()
    {
        flashMaterial = new Material(flashMaterial);
        InitMaterial();
    }

    private void InitMaterial()
    {
        if(spriteRenderer != null) originalMaterial = spriteRenderer.material;
        
    }
    public void Flash(Color color)
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        if(!gameObject.activeInHierarchy) return;
        flashRoutine = StartCoroutine(FlashRoutine(color));
    }

    private IEnumerator FlashRoutine(Color color)
    {
        spriteRenderer.material = flashMaterial;

        flashMaterial.color = color;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}
