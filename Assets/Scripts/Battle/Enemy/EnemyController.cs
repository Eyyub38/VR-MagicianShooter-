using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EnemyController : MonoBehaviour {
    [SerializeField] Enemy enemy;

    float enemyHealth;
    float currSpeed;
    float lastHitTime = 0f;

    HealthBar healthBar;
    ScoreBoard scoreBoard;
    EnemySpawner spawner;
    Player target;
    SpellElementChart spellElementChart;

    public float EnemyHealth { get { return enemyHealth; } set { enemyHealth = value; } }
    public Enemy Enemy { get { return enemy; } set { enemy = value; } }


    public static event Action<float> OnDeath;

    void Awake() {
        scoreBoard = FindFirstObjectByType<ScoreBoard>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start() {
        currSpeed = enemy.MoveSpeed;
        spawner = GameObject.FindFirstObjectByType<EnemySpawner>();
        target = GameObject.FindFirstObjectByType<Player>();
        enemyHealth = enemy.EnemyMaxHealth;
        healthBar.UpdateHealthBar( enemyHealth, enemy.EnemyMaxHealth );
        spellElementChart = new SpellElementChart();
    }

    void Update() {
        if(GameManager.i.CurrentGameState != GameStates.Playing) {
            return;
        }

        if(UnityEngine.XR.XRSettings.isDeviceActive && healthBar != null) {
            healthBar.transform.LookAt( Camera.main.transform );
            healthBar.transform.Rotate( 0, 180, 0 );
        }

        var targetPos = target.transform.position;
        FacePlayer( target.transform );
        Vector3 targetDistance = new Vector3( targetPos.x - transform.position.x, 0f, targetPos.z - transform.position.z );
        Vector3 direction = (targetDistance).normalized;
        if(targetDistance.magnitude >= enemy.MinDistanceToTarget) {
            transform.position += direction * currSpeed * Time.deltaTime;
        }

        if(targetDistance.magnitude <= enemy.MinDistanceToTarget) {
            lastHitTime += Time.deltaTime;
            if(lastHitTime >= enemy.HitCooldown) {
                GiveDamage();
                lastHitTime = 0f;
                target.HealthBar.UpdateHealthBar( target.PlayerHealth, target.MaxHealth );
            }
        }

        if(enemyHealth <= 0) {
            KillEnemy();
        }
    }

    public void TakeDamage(float damage) {
        float finalDamage = damage;

        enemyHealth -= finalDamage;
        healthBar.UpdateHealthBar( enemyHealth, enemy.EnemyMaxHealth );
    }

    public void TakeDamage(Spell spell) {
        float damage = spell.DamageDealt;
        float multiplier = spellElementChart.GetDamageMultiplier( enemy.Element, spell.SpellType.Element );

        damage *= multiplier;
        enemyHealth -= damage;
        healthBar.UpdateHealthBar( enemyHealth, enemy.EnemyMaxHealth );
    }

    void OnTriggerEnter(Collider trigger) {
        if(trigger.gameObject.CompareTag( "Spell" )) {
            Spell spell = trigger.gameObject.GetComponent<Spell>();
            TakeDamage( spell );
        }
    }

    void GiveDamage() {
        target.PlayerHealth -= enemy.Damage;
        if(target.PlayerHealth <= 0) {
            target.Die();
        }
    }

    void KillEnemy() {
        Destroy( gameObject );
        float points = enemy.PointGain;
        OnDeath?.Invoke( points );
        scoreBoard.IncreaseScore( points );
        spawner.EnemiesSpawned--;
        spawner.IsAnyEnemyAlive();
    }

    public void ApplySlow(float value, float duration) {
        StartCoroutine( SlowEnemy( value, duration ) );
    }

    IEnumerator SlowEnemy(float multipiler, float duration) {
        currSpeed = enemy.MoveSpeed * multipiler;
        yield return new WaitForSeconds( duration );
        currSpeed = enemy.MoveSpeed;
    }

    public void StartDamageOverTime(float damage, float duration, float period) {
        StartCoroutine( DamageOverTime( damage, duration, period ) );
    }

    IEnumerator DamageOverTime(float damage, float duration, float period) {
        float time = 0;
        while(time < duration) {
            TakeDamage( damage );
            yield return new WaitForSeconds( period );
            time += period;
        }
    }

    void FacePlayer(Transform target) {
        Vector3 targetPos = new Vector3( target.position.x, transform.position.y / 2, target.position.z );
        transform.LookAt( targetPos );
    }
}
