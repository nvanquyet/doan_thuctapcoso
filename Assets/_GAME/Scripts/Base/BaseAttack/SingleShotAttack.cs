using ShootingGame;
using System.Collections.Generic;
using UnityEngine;

public abstract class AShootAttack : AttackBehaviour
{
    [SerializeField] protected Projectile projectilePrefab;
    private ObjectPooling<Projectile> projectilePooling;
    private List<Projectile> projectileActived;


    private void Start()
    {
        projectilePooling = new ObjectPooling<Projectile>(projectilePrefab, 10, transform);
        projectileActived = new List<Projectile>();
    }

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
        projectile.Spawn(direction, param, owner);
        projectile.OnRecycle = () => {
            if (projectile == null) return;
            else
            {
                projectile.transform.SetParent(transform);
                if(projectileActived.Contains(projectile)) projectileActived.Remove(projectile);
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

    public override void ExecuteAttack()
    {
        if (target is MonoBehaviour)
        {
            var direction = ((target as MonoBehaviour).transform.position - transform.position).normalized;
            FirePoint.position = transform.position + direction.normalized * 1.1f;

            direction = (Vector2) direction;
            SpawnProjectile(direction, FirePoint.position, impactData);
        }
    }
}
