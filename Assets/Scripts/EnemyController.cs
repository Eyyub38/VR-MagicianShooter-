using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyMaxHealth = 10f;
    [SerializeField] float minDistanceToTarget = 1f;
    [SerializeField] float damageToPlayer = 1f;
    [SerializeField] float hitCooldown = 1f;
    [SerializeField] Image healthBar;

    float healthbarLength;
    float enemyHealth;


    float lastHitTime = 0f;
    EnemySpawner spawner;

    PlayerController target;

    public float EnemyHealth { get { return enemyHealth; } set { enemyHealth = value; } }

    void Start()
    {
        spawner = GameObject.FindFirstObjectByType<EnemySpawner>();
        target = GameObject.FindFirstObjectByType<PlayerController>();
        healthbarLength = healthBar.rectTransform.sizeDelta.x;
        enemyHealth = enemyMaxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (GameManager.i.CurrentGameState != GameStates.Playing)
        {
            return;
        }

        var targetPos = target.transform.position;
        Vector3 targetDistance = targetPos - transform.position;
        Vector3 direction = (targetPos - transform.position).normalized;
        if (targetDistance.magnitude >= minDistanceToTarget)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        if (targetDistance.magnitude <= minDistanceToTarget)
        {
            lastHitTime += Time.deltaTime;
            if (lastHitTime >= hitCooldown)
            {
                GiveDamage();
                lastHitTime = 0f;
                target.UpdateHealthBar();
            }
        }

        if (enemyHealth <= 0)
        {
            KillEnemy();
        }
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        UpdateHealthBar();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spell"))
        {
            Spell spell = collision.gameObject.GetComponent<Spell>();
            TakeDamage(spell.DamageDealt);
        }
    }

    void GiveDamage()
    {
        target.PlayerHealth -= damageToPlayer;
        if (target.PlayerHealth <= 0)
        {
            target.Die();
        }
    }

    void KillEnemy()
    {
        Destroy(gameObject);
        spawner.EnemiesSpawned--;
        spawner.IsAnyEnemyAlive();
    }

    public void UpdateHealthBar()
    {
        float healthRation = enemyHealth / enemyMaxHealth;
        healthRation = Mathf.Clamp(healthRation, 0f, 1f);
        healthBar.rectTransform.sizeDelta = new Vector2(healthbarLength * healthRation, healthBar.rectTransform.sizeDelta.y);
    }
}
