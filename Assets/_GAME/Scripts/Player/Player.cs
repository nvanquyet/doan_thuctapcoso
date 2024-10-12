using System;
using UnityEngine;
using ShootingGame;
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
            SetStatForPlayer();
            InitData();

            RegisterEventListener();
        }

        private void RegisterEventListener()
        {
            this.AddListener<GameEvent.OnStatChange>(OnStatChange, false);
        }

        private void OnStatChange(GameEvent.OnStatChange param)
        {
            SetStatForPlayer();
        }

        private void SetStatForPlayer()
        {
            if (_playerStat == null) return;
            var currentData = _playerStat.CurrentStat;
            _playerDefender.SetHealth((int) currentData.GetStat(ShootingGame.Data.TypeStat.Hp).Value, false);
            _playerMovement.SetSpeed(currentData.GetStat(ShootingGame.Data.TypeStat.MoveSpeed).Value);
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