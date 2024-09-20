using Unity.VisualScripting;
using UnityEngine;
using VawnWuyest.Data;
namespace ShootingGame
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : AInteractor
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private PlayerDefender _playerDefender;

        /// <summary>
        /// You must have StatData in the Player
        /// </summary>
        [SerializeField] private PlayerStatData _statData;

        //public PlayerStatData StatData => _statData;

        public PlayerStat Stat => _statData.GetAllValue()[0];

        private void OnValidate()
        {
            _playerSpawner = GetComponentInChildren<PlayerSpawner>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerDefender = GetComponent<PlayerDefender>();
        }

        void Start()
        {
            GameCtrl.Instance.AddPlayer(this);
            if(_playerSpawner != null && _playerMovement != null) {
                _playerSpawner.Spawn();
                _playerMovement.Init(_playerSpawner.transform);
                _playerSpawner.Init(_playerMovement);
            }

            _playerDefender.Init();
        }
    }

}