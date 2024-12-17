using ShootingGame;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : Frame
{
    [SerializeField] private Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() => Hide());
    }
}
