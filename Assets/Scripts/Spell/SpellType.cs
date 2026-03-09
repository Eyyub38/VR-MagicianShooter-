using UnityEngine;

public enum SpellElement { Fire, Ice, Lightning }

[CreateAssetMenu(fileName = "New Spell Type", menuName = "Spell/Spell Type")]
public class SpellType : ScriptableObject
{
    [SerializeField] float spellDamage;
    [SerializeField] float spellMinSize;
    [SerializeField] float spellMaxSize;
    [SerializeField] float spellGrowthRate;
    [SerializeField] float spellDuration;
    [SerializeField] float manaCost;
    [SerializeField] float cooldown;
    [SerializeField] SpellElement element;
    //[SerializeField] SpellEffects spellEffect;

    // Base Stats
    public float SpellDamage => spellDamage;
    public float SpellMinSize => spellMinSize;
    public float SpellMaxSize => spellMaxSize;
    public float SpellGrowthRate => spellGrowthRate;
    public float SpellDuration => spellDuration;

    // Type and Cost !!TODO: Maybe move these to a separate interface for casting?
    public SpellElement Element => element;
    public float ManaCost => manaCost;
    public float Cooldown => cooldown;
    //public SpellEffects SpellEffect => spellEffect;
}
