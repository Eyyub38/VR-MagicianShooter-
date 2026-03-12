using UnityEngine;

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