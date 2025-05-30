using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector] public int currentHealth;

    public ProgressBar healthBar;

    private float safeTime;
    public float safeTimeDuration = 0f;
    public bool isDead = false;

    public bool camShake = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.UpdateProgess(currentHealth, maxHealth);
    }

    public void TakeDam(int damage)
    {
        if (safeTime <= 0)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                if (this.gameObject.tag == "Enemy")
                {
                    //WeaponCtrl.Instance.RemoveEnemyToFireRange(this.transform);
                    //FindObjectOfType<Killed>().UpdateKilled();
                    //FindObjectOfType<PlayerExp>().UpdateExperience(UnityEngine.Random.Range(1, 4));
                    Destroy(this.gameObject, 0.125f);
                }
                isDead = true;
            }

            // If player then update health bar
            if (healthBar != null)
                healthBar.UpdateProgess(currentHealth, maxHealth);

            safeTime = safeTimeDuration;
        }
    }

    private void Update()
    {
        if (safeTime > 0)
        {
            safeTime -= Time.deltaTime;
        }
    }
}
