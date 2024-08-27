using System;
using UnityEngine;
namespace ShootingGame
{
    public interface ISpawner
    {
        void Spawn();
    }

    public interface IPlayerSpawner : ISpawner
    {
        PlayerGraphic PlayerGraphic { get; }
    }


    public class PlayerSpawner : MonoBehaviour, IPlayerSpawner
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


        protected virtual string GetPlayerGraphicID()
        {
            return "Player1";
        }

        internal void Init(IPlayerMovement playerMovement)
        {
            if(PlayerGraphic != null) PlayerGraphic.Init(playerMovement);
        }
    }
}