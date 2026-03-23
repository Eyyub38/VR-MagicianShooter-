using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu( fileName = "New Spell Type", menuName = "Spell/Spell Type" )]
public class SpellType : ScriptableObject {
    [SerializeField] float spellDamage;
    [SerializeField] float spellMinSize;
    [SerializeField] float spellMaxSize;
    [SerializeField] float spellGrowthRate;
    [SerializeField] float spellDuration;
    [SerializeField] float manaCost;
    [SerializeField] float cooldown;
    [SerializeField] ElementData element;

    [SerializeField] List<SpellEffect> effects;

    // Base Stats
    public float SpellDamage => spellDamage;
    public float SpellMinSize => spellMinSize;
    public float SpellMaxSize => spellMaxSize;
    public float SpellGrowthRate => spellGrowthRate;
    public float SpellDuration => spellDuration;
    public ElementData Element => element;


    // Type and Cost !!TODO: Maybe move these to a separate interface for casting?
    public float ManaCost => manaCost;
    public float Cooldown => cooldown;

    public void TriggerEffects(GameObject target) {
        foreach(var effect in effects) {
            effect.Apply( target );
        }

        if(target.TryGetComponent<EnemyElementProcessor>( out var processor )) {
            processor.OnHit( element );
        }
    }
}
