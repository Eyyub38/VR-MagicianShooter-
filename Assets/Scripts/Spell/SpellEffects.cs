using System;
using UnityEngine;

public class SpellEffects : ScriptableObject
{
    public string effectName { get; set; }
    public float effectDamage { get; set; }
    public float effectDuration { get; set; }
    public float effectPeriod { get; set; }
    public SpellID spellID { get; set; }

    public Action<EnemyController, float, float, float> OnDamageEffects { get; set; }
    public Action<EnemyController, float, float, float> OnMovingEffects { get; set; }
}
