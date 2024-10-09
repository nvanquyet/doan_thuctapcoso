using DG.Tweening;
using UnityEngine;
namespace ShootingGame
{

    public class WeaponSpawnPos : MonoBehaviour
    {
        [SerializeField] private Vector3 localPosition;
        [SerializeField] private Vector3 localEuLerAngles;

        private void OnValidate()
        {
            #if UNITY_EDITOR
                localPosition = transform.localPosition;
                localEuLerAngles = transform.localEulerAngles;
            #endif
        }
        
        // Start is called before the first frame update
        void Start()
        {
            this.AddListener<GameEvent.OnWaveClear>(OnWaveClear);
        }

        private void OnWaveClear(GameEvent.OnWaveClear clear)
        {
            transform.DOLocalMove(localPosition, 0.2f);
            transform.DOLocalRotate(localEuLerAngles, 0.2f);
        }
    }

}   