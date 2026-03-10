using UnityEngine;

public enum EnemyType { Normal, Brute, Fast }

[CreateAssetMenu( fileName = "New Enemy", menuName = "Enemy" )]
public class Enemy : ScriptableObject {
    [SerializeField] EnemyType enemyType;
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyMaxHealth = 10f;
    [SerializeField] float minDistanceToTarget = 1f;
    [SerializeField] float damage = 1f;
    [SerializeField] float hitCooldown = 1f;
    [SerializeField] float pointGian = 1f;

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float EnemyMaxHealth => enemyMaxHealth;
    public float MinDistanceToTarget => minDistanceToTarget;
    public float Damage => damage;
    public float HitCooldown => hitCooldown;
    public float PointGain => pointGian;
}
