using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SpellMachine : MonoBehaviour {
    List<SpellData> spells = new List<SpellData>();
    public Dictionary<ElementID, SpellType> spellCache = new Dictionary<ElementID, SpellType>();

    public static SpellMachine i { get; private set; }

    void Awake() {
        i = this;
    }

    void Start() {
        spellCache.Add( ElementID.Fire, Resources.Load<SpellType>( "Scriptibles/Spells/Types/FireBall" ) );
        spellCache.Add( ElementID.Ice, Resources.Load<SpellType>( "Scriptibles/Spells/Types/IceBall" ) );
        spellCache.Add( ElementID.Wind, Resources.Load<SpellType>( "Scriptibles/Spells/Types/WindBall" ) );
        spellCache.Add( ElementID.Lightning, Resources.Load<SpellType>( "Scriptibles/Spells/Types/LightningBall" ) );
        spellCache.Add( ElementID.Earth, Resources.Load<SpellType>( "Scriptibles/Spells/Types/EarthBall" ) );
    }

    public SpellData GetRandomSpellData() {
        if(spells.Count == 0) {
            if(WaveManager.i != null && WaveManager.i.CurrentWave != null) {
                CheckAndRefillBag( WaveManager.i.CurrentWave );
            }
        }

        if(spells.Count == 0) return null;

        int spellIndex = Random.Range( 0, spells.Count );
        return spells[spellIndex];
    }

    public void ConsumeSpellData(SpellData spellData) {
        if(spellData == null) return;

        spellData.quantity--;

        if(spellData.quantity <= 0) {
            spells.Remove( spellData );
        }
    }

    public void CheckAndRefillBag(WaveData wave) {
        spells.Clear();

        foreach(WaveEnemyData enemyData in wave.EnemyData) {
            if(enemyData.Quantity <= 0) continue;

            ElementID weakSpellElement = GetSpellFromEnemy( enemyData.Element );

            var existingSpell = spells.Find( s => s.element == weakSpellElement );

            if(existingSpell == null) {
                spells.Add( new SpellData {
                    element = weakSpellElement,
                    quantity = enemyData.Quantity
                } );
            } else {
                existingSpell.quantity = Mathf.Max( existingSpell.quantity, enemyData.Quantity );
            }
        }
    }

    public void CleanBag() {
        spells.RemoveAll( s => s.quantity <= 0 );
    }

    ElementID GetSpellFromEnemy(ElementID enemyElement) {
        switch(enemyElement) {
            case ElementID.Fire:
                return ElementID.Earth;
            case ElementID.Ice:
                return ElementID.Fire;
            case ElementID.Wind:
                return ElementID.Ice;
            case ElementID.Lightning:
                return ElementID.Wind;
            case ElementID.Earth:
                return ElementID.Lightning;
            default:
                return ElementID.None;
        }
    }
}

public class SpellData {
    public ElementID element;

    public int quantity;
}