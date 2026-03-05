using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] float spellMaxSize;
    [SerializeField] float spellGrowthRate;
    [SerializeField] float spellForce;
    [SerializeField] float spellDuration;
    [SerializeField] float spellDamage;
    [SerializeField] float spellMinSize;

    bool isGrowing = true;

    Rigidbody rigidBody;

    float spellSize;
    float currSize;
    float damageDealt;

    public float SpellForce => spellForce;
    public float DamageDealt => damageDealt;
    public float CurrSize => currSize;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
    }

    public void Init()
    {
        spellSize = spellMinSize;
        transform.localScale = Vector3.zero;
        damageDealt = spellDamage;
    }

    void Update()
    {
        if (isGrowing)
        {
            currSize = transform.localScale.x;
            spellSize = Mathf.MoveTowards(currSize, spellMaxSize, spellGrowthRate * Time.deltaTime);
            transform.localScale = Vector3.one * spellSize;
        }
    }

    public void Cast(Vector3 direction, float force)
    {
        isGrowing = false;
        transform.SetParent(null);
        /*
        rigidBody.isKinematic = false;
        var trueForce = direction * force * currSize * 1250f;
        rigidBody.AddForce(trueForce);
        */
        rigidBody.isKinematic = false;
        rigidBody.linearVelocity = direction * force * currSize * 25f;
        damageDealt = spellDamage * (currSize / spellMaxSize);

        DestroySpell();
    }

    public void StopCast()
    {
        isGrowing = false;
        transform.localScale = spellSize * Vector3.one;
    }

    void DestroySpell()
    {
        Destroy(gameObject, spellDuration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}