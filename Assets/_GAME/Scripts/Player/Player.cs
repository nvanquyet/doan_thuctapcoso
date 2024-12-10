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
            AddListener();
        }


        private void OnNextWave(GameEvent.OnNextWave param)
        {
            _playerStat.OnNextWave(param);
            _playerMovement.PauseMovement(false);
            progesstion.IsLevelUp = false;
        }
        private void OnWaveClear(GameEvent.OnWaveClear param)
        {
            _playerMovement.PauseMovement(true);
        }

        public void GainExp(int exp) => progesstion.AddEXP(exp);


        private void OnStatChanged(StatContainerData CurrentStat)
        {
            if(CurrentStat == null) return;
            _playerDefender.SetHealth((int)CurrentStat.GetStat(ShootingGame.Data.TypeStat.Hp).Value, false);
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
            });
        }

        private void AddListener()
        {
            this.AddListener<GameEvent.OnNextWave>(OnNextWave, false);
            this.AddListener<GameEvent.OnWaveClear>(OnWaveClear, false);
        }
    }


}