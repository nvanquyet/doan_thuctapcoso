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
        if (target is MonoBehaviour)
        {
            var direction = ((target as MonoBehaviour).transform.position - transform.position).normalized;
            FocusPoint.position = transform.position + direction.normalized;
            FocusPoint.right = direction;
        }

        foreach (var firePoint in FirePoints)
        {
            SpawnProjectile((firePoint.position - FocusPoint.position).normalized, FocusPoint.position, impactData);
        }

    }
}
