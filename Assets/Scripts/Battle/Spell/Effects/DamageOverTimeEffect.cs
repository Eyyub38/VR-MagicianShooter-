using UnityEngine;

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