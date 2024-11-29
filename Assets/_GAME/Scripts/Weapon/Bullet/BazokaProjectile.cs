using ShootingGame;
using UnityEngine;
using static ShootingGame.Interface;

public class BazokaProjectile : Projectile
{
    [SerializeField] private float explosionRadius = 5f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.TryGetComponent(out Interface.IDefender defender))
        {
            if (originatingOwner != null && defender.GetType().Equals(originatingOwner.GetType())) return;
            Collider2D[] affected = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D collider in affected)
            {
                if (collider.TryGetComponent(out IDefender defend))
                {
                    if(defend.GetType().Equals(originatingOwner.GetType())) continue;
                    Attack(defend);
                }
            }
            
            ActivateEffect(impactEffect);
            Rigidbody.velocity = Vector2.zero;

        }
    }

}
