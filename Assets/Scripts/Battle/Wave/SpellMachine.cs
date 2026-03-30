using UnityEngine;
using System.Collections.Generic;

public class SpellMachine : MonoBehaviour {
    List<SpellData> spells = new List<SpellData>();
    Dictionary<ElementID, SpellType> spellCache = new Dictionary<ElementID, SpellType>();

    void Start() {
        spellCache.Add( ElementID.Fire, Resources.Load<SpellType>( "Scriptibles/Spells/Types/FireBall" ) );
        spellCache.Add( ElementID.Ice, Resources.Load<SpellType>( "Scriptibles/Spells/Types/IceBall" ) );
        spellCache.Add( ElementID.Wind, Resources.Load<SpellType>( "Scriptibles/Spells/Types/WindBall" ) );
        spellCache.Add( ElementID.Lightning, Resources.Load<SpellType>( "Scriptibles/Spells/Types/LightningBall" ) );
        spellCache.Add( ElementID.Earth, Resources.Load<SpellType>( "Scriptibles/Spells/Types/EarthBall" ) );
    }

    public void CheckAndRefillBag(WaveData wave) {
        foreach(WaveEnemyData enemyData in wave.EnemyData) {
            var existingSpell = spells.Find( s => s.element == enemyData.Element );

            if(existingSpell == null || existingSpell.quantity <= 0) {

                if(existingSpell == null) {
                    spells.Add( new SpellData {
                        element = enemyData.Element,
                        quantity = enemyData.Quantity
                    } );
                } else {
                    existingSpell.quantity = enemyData.Quantity;
                }

                Debug.Log( $" Added {enemyData.Quantity} spells of type {enemyData.Element} enemies " );
            }
        }
    }

    public void CleanBag() {
        spells.RemoveAll( s => s.quantity <= 0 );
    }

    public SpellType GetSpellFromEnemy(WaveEnemyData enemyData) {
        switch(enemyData.Element) {
            case ElementID.Fire:
                return spellCache[ElementID.Earth];
            case ElementID.Ice:
                return spellCache[ElementID.Fire];
            case ElementID.Wind:
                return spellCache[ElementID.Ice];
            case ElementID.Lightning:
                return spellCache[ElementID.Wind];
            case ElementID.Earth:
                return spellCache[ElementID.Lightning];
            default:
                return null;
        }
    }
}

public class SpellData {
    public ElementID element;
    public float quantity;
}