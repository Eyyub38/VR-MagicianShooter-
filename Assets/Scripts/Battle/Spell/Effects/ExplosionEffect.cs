using UnityEngine;

[CreateAssetMenu( fileName = "New Spell Effect", menuName = "Spell/Spell Effect/Explosion" )]
public class ExplosionEffect : SpellEffect {
    public float range;
    public float damage;

    public override void Apply(GameObject target) {
        Collider[] hits = Physics.OverlapSphere( target.transform.position, range );

        foreach(var hit in hits) {
            if(hit.TryGetComponent<EnemyController>( out var enemy )) {
                enemy.TakeDamage( damage );
            }
        }
    }
}