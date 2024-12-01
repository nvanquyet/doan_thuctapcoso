using UnityEngine;
using ShootingGame;
public class SpreadShotAttack : AShootAttack
{
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private Transform focusPoint;

    public Transform[] FirePoints
    {
        get
        {
            if (firePoints == null) return new Transform[] { transform };
            return firePoints;
        }
    }

    public Transform FocusPoint
    {
        get
        {
            if (focusPoint == null) return transform;
            return focusPoint;
        }
    }

    public override void ExecuteAttack()
    {
        foreach (var firePoint in FirePoints)
        {
            SpawnProjectile((FocusPoint.position - firePoint.position).normalized, FocusPoint.position, impactData);
        }
    }
}
