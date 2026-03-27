using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ElementID { None, Fire, Lightning, Ice, Earth, Wind }

[CreateAssetMenu( fileName = "New Spell Type", menuName = "Spell/Spell Type" )]
public class SpellType : ScriptableObject {
    [SerializeField] float spellDamage;
    [SerializeField] float spellMinSize;
    [SerializeField] float spellMaxSize;
    [SerializeField] float spellGrowthRate;
    [SerializeField] float spellDuration;

    [SerializeField] ElementID element;
    [SerializeField] List<SpellEffect> effects;

    public float SpellDamage => spellDamage;
    public float SpellMinSize => spellMinSize;
    public float SpellMaxSize => spellMaxSize;
    public float SpellGrowthRate => spellGrowthRate;
    public float SpellDuration => spellDuration;
    public ElementID Element => element;

    public void TriggerEffects(GameObject target) {
        foreach(var effect in effects) {
            effect.Apply( target );
        }
    }
}

public class SpellElementChart {
    public Dictionary<ElementID, Dictionary<ElementID, float>> damageMultiplier = new Dictionary<ElementID, Dictionary<ElementID, float>>() {
        {ElementID.Fire ,new Dictionary<ElementID,float> {{ElementID.Ice, 1.5f}, {ElementID.Lightning, 1.25f}, {ElementID.Wind, 0.75f}, {ElementID.Earth, 0.5f}}},
        {ElementID.Ice, new Dictionary<ElementID,float> {{ElementID.Wind, 1.5f}, {ElementID.Earth, 1.25f}, {ElementID.Lightning, 0.75f}, {ElementID.Fire, 0.5f}}},
        {ElementID.Wind, new Dictionary<ElementID, float> {{ElementID.Lightning, 1.5f},{ElementID.Earth, 1.25f}, {ElementID.Fire, 0.75f}, {ElementID.Ice, 0.5f}}},
        {ElementID.Lightning, new Dictionary<ElementID, float> {{ElementID.Earth, 1.5f}, {ElementID.Fire, 1.25f}, {ElementID.Ice, 0.75f}, {ElementID.Wind, 0.5f}}},
        {ElementID.Earth, new Dictionary<ElementID, float> {{ElementID.Fire, 1.5f}, {ElementID.Ice, 1.25f}, {ElementID.Wind, 0.75f}, {ElementID.Lightning, 0.5f}}}
    };

    public float GetDamageMultiplier(ElementID attacker, ElementID target) {
        if(damageMultiplier.TryGetValue( attacker, out var targets ) && targets.TryGetValue( target, out float multiplier )) {
            return multiplier;
        }
        return 1f;
    }
}