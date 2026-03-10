using UnityEngine;

public class Spell : MonoBehaviour {
    [SerializeField] SpellType spellType;
    [SerializeField] float speedMultiplier;

    bool isGrowing = false;
    float spellSize = 0f;
    float currSize = 0f;
    float damageDealt = 0f;

    Vector3 spellVelocity;

    public float DamageDealt => damageDealt;

    public void Init() {
        isGrowing = true;
        spellSize = spellType.SpellMinSize;
        transform.localScale = Vector3.zero;
        damageDealt = spellType.SpellDamage;
    }

    void Update() {
        transform.position += spellVelocity * Time.deltaTime;

        if(isGrowing) {
            currSize = transform.localScale.x;
            spellSize = Mathf.MoveTowards( currSize, spellType.SpellMaxSize, spellType.SpellGrowthRate * Time.deltaTime );
            transform.localScale = Vector3.one * spellSize;
        }
    }

    public void Cast(Vector3 direction) {
        transform.SetParent( null );
        spellVelocity = direction * currSize * speedMultiplier;
        damageDealt = spellType.SpellDamage * (currSize / spellType.SpellMaxSize);

        DestroySpell();
    }

    public void StopCast() {
        isGrowing = false;
        transform.localScale = spellSize * Vector3.one;
    }

    void DestroySpell() {
        Destroy( gameObject, spellType.SpellDuration );
    }

    void OnTriggerEnter(Collider trigger) {
        if(trigger.gameObject.CompareTag( "Enemy" )) {
            spellType.TriggerEffects( trigger.gameObject );
            Destroy( gameObject );
        }
    }

    public void Tick(float deltaTime) {
        //Particle effects and cooldown would go here
    }

    public bool IsReady() {
        //Cooldown and mana checks would go here
        return true;
    }
}