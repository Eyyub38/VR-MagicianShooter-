using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] Image healthBar;
    ScoreBoard scoreBoard;

    float healthbarLength;
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
        healthbarLength = healthBar.rectTransform.sizeDelta.x;
        enemyHealth = enemy.EnemyMaxHealth;
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

    public void UpdateHealthBar()
    {
        float healthRation = enemyHealth / enemy.EnemyMaxHealth;
        healthRation = Mathf.Clamp(healthRation, 0f, 1f);
        healthBar.rectTransform.sizeDelta = new Vector2(healthbarLength * healthRation, healthBar.rectTransform.sizeDelta.y);
    }
}
