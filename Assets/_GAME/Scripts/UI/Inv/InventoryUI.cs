using ShootingGame;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Frame
{
    [SerializeField] private Button btnTetrisInv;
    [SerializeField] private Button btnRandomItem;

    public Action OnButtonTetrisInvClickAction;


    private void Start()
    {
        btnTetrisInv.onClick.AddListener(OnButtonTetrisInvClick);
        btnRandomItem.onClick.AddListener(OnButtonRandomItemClick);
    }

    private void OnButtonRandomItemClick()
    {
        //Random Item Here

    }

    private void OnButtonTetrisInvClick() => OnButtonTetrisInvClickAction?.Invoke();
}
