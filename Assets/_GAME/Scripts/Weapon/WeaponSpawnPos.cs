using DG.Tweening;
using UnityEngine;
using static GameEvent;
namespace ShootingGame
{

    public class WeaponSpawnPos : MonoBehaviour
    {
        [SerializeField] private Vector3 localPosition;
        [SerializeField] private Vector3 localEuLerAngles;

#if UNITY_EDITOR
        private void OnValidate()
        {
            localPosition = transform.localPosition;
            localEuLerAngles = transform.localEulerAngles;
        }
#endif

        // Start is called before the first frame update
        void Start()
        {
            this.AddListener<GameEvent.OnWaveClear>(OnWaveClear, false);
            this.AddListener<GameEvent.OnNextWave>(OnNextWave, false);
        }

        private void OnWaveClear(GameEvent.OnWaveClear param)
        {
            transform.localPosition = localPosition;
            transform.localEulerAngles = localEuLerAngles;
        }
        private void OnNextWave(GameEvent.OnNextWave param)
        {
            transform.localPosition = localPosition;
            transform.localEulerAngles = localEuLerAngles;
        }
    }

}