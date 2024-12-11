using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerStat), typeof(LevelProgesstion))]
    public class Player : AInteractor, Interface.IExpReceiver
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private PlayerDefender _playerDefender;
        [SerializeField] private PlayerStat _playerStat;
        [SerializeField] private LevelProgesstion progesstion;

        public bool IsLevelUp => progesstion.IsLevelUp;

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
            progesstion = GetComponent<LevelProgesstion>();
        }
#endif


        void Start()
        {
            GameCtrl.Instance.AddPlayer(this);
            progesstion.Initialized();
            InitAction();
        }


        public void OnNextWave(GameEvent.OnNextWave param)
        {
            _playerStat.OnNextWave(param);
            progesstion.IsLevelUp = false;
        }
        public void GainExp(int exp) => progesstion.AddEXP(exp);


        private void OnStatChanged(StatContainerData CurrentStat)
        {
            if(CurrentStat == null) return;
            var maxHealth = (int)CurrentStat.GetStat(ShootingGame.Data.TypeStat.Hp).Value;
            var healthBuff = Mathf.Max(maxHealth - _playerDefender.MaxHealth, 0);
            _playerDefender.SetHealth(maxHealth, false);
            _playerDefender.BuffHealth(healthBuff);
            _playerMovement.SetSpeed(CurrentStat.GetStat(ShootingGame.Data.TypeStat.MoveSpeed).Value);
        }


        private void InitAction()
        {
            _playerStat.Initialized(OnStatChanged);
            if (_playerSpawner != null && _playerMovement != null)
            {
                _playerSpawner.Spawn();
                _playerMovement.Init(_playerSpawner.transform);
                _playerSpawner.Init(_playerMovement);
            }
            _playerDefender.Init(_playerStat, () =>
            {
                _playerSpawner.PlayerGraphic.OnHitAnimation();
            }, () =>
            {
                GameCtrl.Instance.OnPlayerDead(this);
            });
        }

    }


}