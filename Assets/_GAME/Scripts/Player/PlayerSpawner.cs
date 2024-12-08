using System;
using UnityEngine;
namespace ShootingGame
{
    public class PlayerSpawner : MonoBehaviour, Interface.IPlayerSpawner
    {

        #region Properties
        [SerializeField] private PlayerGraphic _playerGraphic;
        #endregion


        #region  Implement
        public PlayerGraphic PlayerGraphic => _playerGraphic;
        public void Spawn()
        {
            //Get the player prefab in GameData with id


            //Finally set _playerGraphic
        }
        #endregion

        internal void Init(Interface.IPlayerMovement playerMovement)
        {
            if(PlayerGraphic != null) PlayerGraphic.Init(playerMovement);
        }
    }
}