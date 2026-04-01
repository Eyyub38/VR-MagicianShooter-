using UnityEngine;

[CreateAssetMenu( fileName = "New Enemy", menuName = "Enemy" )]
public class Enemy : ScriptableObject {
    [SerializeField] float moveSpeed;
    [SerializeField] float enemyMaxHealth = 10f;
    [SerializeField] float minDistanceToTarget = 1f;
    [SerializeField] float damage = 1f;
    [SerializeField] float hitCooldown = 1f;
    [SerializeField] float pointGain = 1f;


    [SerializeField] ElementID element;

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public ElementID Element { get { return element; } set { element = value; } }
    public float EnemyMaxHealth => enemyMaxHealth;
    public float MinDistanceToTarget => minDistanceToTarget;
    public float Damage => damage;
    public float HitCooldown => hitCooldown;
    public float PointGain => pointGain;
}
