using UnityEngine;

public enum EnemyType { Magma, Blizzard, Magnet }

[CreateAssetMenu( fileName = "New Enemy", menuName = "Enemy" )]
public class Enemy : ScriptableObject {
    [SerializeField] EnemyType enemyType;
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyMaxHealth = 10f;
    [SerializeField] float minDistanceToTarget = 1f;
    [SerializeField] float damage = 1f;
    [SerializeField] float hitCooldown = 1f;
    [SerializeField] float pointGian = 1f;
    [SerializeField] ElementData weakness;
    [SerializeField] ElementData resistance;

    [Range( 0, 100 )]
    [SerializeField] int sapwnWeight = 50;

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float EnemyMaxHealth => enemyMaxHealth;
    public float MinDistanceToTarget => minDistanceToTarget;
    public float Damage => damage;
    public float HitCooldown => hitCooldown;
    public float PointGain => pointGian;
    public ElementData Weakness => weakness;
    public ElementData Resistance => resistance;
    public int SpawnWeight => sapwnWeight;
}
