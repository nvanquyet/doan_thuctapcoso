using ShootingGame;
using System.Collections.Generic;
using UnityEngine;

public abstract class AShootAttack : AttackBehaviour
{
    [SerializeField] protected Projectile projectilePrefab;
    private ObjectPooling<Projectile> projectilePooling;
    private List<Projectile> projectileActived;

    private void OnDestroy()
    {
        if (projectileActived != null && projectileActived.Count > 0)
        {
            foreach (var projectile in projectileActived)
            {
                Destroy(projectile.gameObject);
            }
        }
    }

    protected void SpawnProjectile(Vector2 direction, Vector3 position, ImpactData param)
    {
        var projectile = projectilePooling.Get();
        projectile.transform.SetParent(null);
        projectile.transform.position = position;
        projectile.Spawn(direction, param, defenderOwner);
        projectile.OnRecycle = () => {
            if (projectile == null) return;
            else
            {
                projectile.transform.SetParent(transform);
                projectileActived.Remove(projectile);
                projectilePooling.Recycle(projectile);
            }
        };
        projectileActived.Add(projectile);
    }
}
public class SingleShotAttack : AShootAttack
{
    [SerializeField] private Transform firePoint;

    
    public Transform FirePoint
    {
        get
        {
            if (firePoint == null) return transform;
            return firePoint;
        }
    }

    public override void ExecuteAttack(Vector2 direction, ImpactData param)
    {
        SpawnProjectile(direction, FirePoint.position, param);
        base.ExecuteAttack(direction, param);
    }
}
