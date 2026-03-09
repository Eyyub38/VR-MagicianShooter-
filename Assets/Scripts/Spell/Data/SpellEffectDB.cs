using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EffectID { Damage, Slow, Stun }

public class SpellEffectDB
{
    public static Dictionary<EffectID, SpellEffects> SpellEffects = new Dictionary<EffectID, SpellEffects>(){
        {EffectID.Damage,
            new SpellEffects(){
                effectName = "Damage",
                effectDamage = 10f,
                effectDuration = 1f,
            }
        }
    };
}
