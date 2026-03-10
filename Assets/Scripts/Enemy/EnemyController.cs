using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] HealthBar healthBar;
    ScoreBoard scoreBoard;

    //TODO:Health System will be in sepsrate script file
    float enemyHealth;

    float lastHitTime = 0f;
    EnemySpawner spawner;

    PlayerController target;

    public float EnemyHealth { get { return enemyHealth; } set { enemyHealth = value; } }

    public static event Action<float> OnDeath;

    void Awake()
    {
        scoreBoard = FindFirstObjectByType<ScoreBoard>();
    }

    void Start()
    {
        spawner = GameObject.FindFirstObjectByType<EnemySpawner>();
        target = GameObject.FindFirstObjectByType<PlayerController>();
        enemyHealth = enemy.EnemyMaxHealth;
        healthBar.UpdateHealthBar(enemyHealth, enemy.EnemyMaxHealth);
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
        if (targetDistance.magnitude >= enemy.MinDistanceToTarget)
        {
            transform.position += direction * enemy.MoveSpeed * Time.deltaTime;
        }

        if (targetDistance.magnitude <= enemy.MinDistanceToTarget)
        {
            lastHitTime += Time.deltaTime;
            if (lastHitTime >= enemy.HitCooldown)
            {
                GiveDamage();
                lastHitTime = 0f;
                target.HealthBar.UpdateHealthBar(target.PlayerHealth, target.MaxHealth);
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
        healthBar.UpdateHealthBar(enemyHealth, enemy.EnemyMaxHealth);
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.CompareTag("Spell"))
        {
            Spell spell = trigger.gameObject.GetComponent<Spell>();
            TakeDamage(spell.DamageDealt);
        }
    }

    void GiveDamage()
    {
        target.PlayerHealth -= enemy.Damage;
        if (target.PlayerHealth <= 0)
        {
            target.Die();
        }
    }

    void KillEnemy()
    {
        Destroy(gameObject);
        float points = enemy.PointGain;
        OnDeath?.Invoke(points);
        scoreBoard.IncreaseScore(points);
        spawner.EnemiesSpawned--;
        spawner.IsAnyEnemyAlive();
    }
}
