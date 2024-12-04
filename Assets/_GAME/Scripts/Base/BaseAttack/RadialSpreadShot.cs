using UnityEngine;

public class RadialSpreadShot : AShootAttack
{
    [SerializeField] private int projectileCount = 10;
    [SerializeField] private float distanceFromObject = 1f;

    public override void ExecuteAttack()
    {
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;

            Vector2 targetPosition = CalculateBulletPosition(transform.position, angle, distanceFromObject);

            var direction =  (targetPosition - (Vector2) transform.position).normalized;

            SpawnProjectile(direction, targetPosition, impactData);
        }
    }

    private Vector3 CalculateBulletPosition(Vector3 position, float angle, float distance)
    {
        float radian = angle * Mathf.Deg2Rad;

        float x = position.x + Mathf.Cos(radian) * distance;
        float y = position.z + Mathf.Sin(radian) * distance;

        return new Vector3(x, y, position.z);
    }
}
