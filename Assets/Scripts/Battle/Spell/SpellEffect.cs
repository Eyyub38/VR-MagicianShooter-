using System;
using UnityEngine;

public abstract class SpellEffect : ScriptableObject {
    public abstract void Apply(GameObject target);
}

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

[CreateAssetMenu( fileName = "New Spell Effect", menuName = "Spell/Spell Effect/Damage Over Time" )]
public class DamageOverTimeEffect : SpellEffect {
    public float duration;
    public float period;
    public float damage;

    public override void Apply(GameObject target) {
        if(target.TryGetComponent<EnemyController>( out var enemy )) {
            enemy.StartDamageOverTime( damage, duration, period );
        }
    }
}

[CreateAssetMenu( fileName = "New Spell Effect", menuName = "Spell/Spell Effect/Slow" )]
public class SlowEffect : SpellEffect {
    public float value;
    public float duration;

    public override void Apply(GameObject target) {
        if(target.TryGetComponent<EnemyController>( out var enemy )) {
            enemy.ApplySlow( value, duration );
        }
    }
}
