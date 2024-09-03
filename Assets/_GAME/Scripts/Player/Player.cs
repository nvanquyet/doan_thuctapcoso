using UnityEngine;
namespace ShootingGame
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : AInteractor
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerSpawner _playerSpawner;


        private void OnValidate()
        {
            _playerSpawner = GetComponentInChildren<PlayerSpawner>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        void Start()
        {
            if(_playerSpawner != null && _playerMovement != null) {
                _playerSpawner.Spawn();
                _playerMovement.Init(_playerSpawner.transform);
                _playerSpawner.Init(_playerMovement);
            }
        }
    }

}