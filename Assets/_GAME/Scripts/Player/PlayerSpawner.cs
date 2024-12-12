using System;
using UnityEngine;
using UnityEngine.UI;
namespace ShootingGame
{
    public class PlayerSpawner : MonoBehaviour, Interface.IPlayerSpawner
    {

        #region Properties
        [SerializeField] private PlayerGraphic _playerGraphic;
        [SerializeField] private Image icon;
        #endregion


        #region  Implement
        public PlayerGraphic PlayerGraphic => _playerGraphic;
        public void Spawn()
        {
            //Get the player prefab in GameData with id
            var player = GameData.Instance.Players.GetValue(UserData.CurrentCharacter);
            if(player)
            {
                if(PlayerGraphic) PlayerGraphic.SetAnimator(player.Animator);
                if(icon && player.Appearance.Icon) icon.sprite = player.Appearance.Icon;
            }
        }
        #endregion

        internal void Init(Interface.IPlayerMovement playerMovement)
        {
            Spawn();
            if (PlayerGraphic != null) PlayerGraphic.Init(playerMovement);
        }
    }
}