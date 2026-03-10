using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SpellID { FireDamage, FireExplotion }

public class SpellEffectDB
{
    public static Dictionary<SpellID, SpellEffects> SpellEffects = new Dictionary<SpellID, SpellEffects>()
    {
        {SpellID.FireDamage,
            new SpellEffects
            {
                effectName = "Fire Damage",
                effectDamage = 10f,
                effectDuration = 1f,
                effectPeriod = 1f,

                OnDamageEffects = (EnemyController target, float effectdamage, float effectPeriod, float effectDuration) => {
                    float timer= 0;
                    if(effectDuration > timer){
                        while (timer % effectPeriod == 0)
                        {
                            target.TakeDamage(effectdamage);
                            timer += Time.deltaTime;
                        }
                    } else return;
                }
            }
        },
        {SpellID.FireExplotion,
            new SpellEffects{
                effectName = "Fire Explosion",
                effectDamage = 10f,
                OnDamageEffects = (EnemyController target, float effectdamage, float range, float timer) => {
                    Collider[] targets = Physics.OverlapSphere(target.transform.position,range);
                    foreach(Collider t in targets){
                        float health = t.GetComponent<EnemyController>().EnemyHealth;
                        if(health > 0){
                            float distance = Vector3.Distance(target.transform.position, t.transform.position);
                            float damageRate = effectdamage - (distance/range);
                            t.GetComponent<EnemyController>().TakeDamage(damageRate);
                        }
                    }
                }
            }
        }
    };
}
