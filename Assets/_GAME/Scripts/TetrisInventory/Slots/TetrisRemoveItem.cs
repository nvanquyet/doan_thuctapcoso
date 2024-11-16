using System;
using UnityEngine;
using static ShootingGame.Interface;
namespace ShootingGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TetrisRemoveItem : UIComponent, Interface.IInteract
    {
        private Action<TetrisItemSlot> OnRemoveItem;
        protected Collider2D _collider;
        public Collider2D Collider
        {
            get
            {
                if(_collider == null) _collider = GetComponent<BoxCollider2D>();
                return _collider;
            }
        }

        public void OnInteract(Interface.IInteract target)
        {

        }

        public void ExitInteract(Interface.IInteract target)
        {
            
        }

        public void SetAction(Action<TetrisItemSlot> action)
        {
            OnRemoveItem = action;
            SetSizeCollider();
        }

        private void SetSizeCollider()
        {
            if(Collider is BoxCollider2D boxCollider2D)
            {
                boxCollider2D.size = new Vector2(RectTransform.rect.width, RectTransform.rect.height);
            }
        }

        public void InvokeAction(TetrisItemSlot target)
        {
            OnRemoveItem?.Invoke(target);
            Destroy(target.gameObject);
        }
    }

}