using System;
using UnityEngine;
using ShootingGame;
using ShootingGame.Data;
namespace ShootingGame
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerStat))]
    public class Player : AInteractor
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private PlayerDefender _playerDefender;
        [SerializeField] private PlayerStat _playerStat;

        public PlayerStat Stat => _playerStat;
        public PlayerDefender Defender => _playerDefender;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _playerSpawner = GetComponentInChildren<PlayerSpawner>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerDefender = GetComponent<PlayerDefender>();
            _playerStat = GetComponent<PlayerStat>();
        }
#endif


        void Start()
        {
            GameCtrl.Instance.AddPlayer(this);
            _playerStat.OnStatChanged = OnStatChanged;
            OnStatChanged(_playerStat.CurrentStat);
            InitData();

        }

        private void OnStatChanged(StatContainerData CurrentStat)
        {
            if(CurrentStat == null) return;
            _playerDefender.SetHealth((int)CurrentStat.GetStat(ShootingGame.Data.TypeStat.Hp).Value, false);
            _playerMovement.SetSpeed(CurrentStat.GetStat(ShootingGame.Data.TypeStat.MoveSpeed).Value);
        }

        private void InitData()
        {
            if (_playerSpawner != null && _playerMovement != null)
            {
                _playerSpawner.Spawn();
                _playerMovement.Init(_playerSpawner.transform);
                _playerSpawner.Init(_playerMovement);
            }
            _playerDefender.Init(_playerStat);
        }
    }


}