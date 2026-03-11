using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour {
    [SerializeField] GameObject spellPrefab;
    [SerializeField] Transform spellSpawnPoint;
    [SerializeField] InputAction castSpellAction;
    [SerializeField] Camera mainCamera;

    Spell currSpell;

    bool isCasting = false;

    void Update() {
        if(GameManager.i == null || GameManager.i.CurrentGameState != GameStates.Playing)
            return;

        if(castSpellAction.WasPressedThisFrame()) {
            ChargeSpell();
        }

        if(castSpellAction.WasReleasedThisFrame() && currSpell != null) {
            CastSpell();
        }
    }

    void ChargeSpell() {
        if(isCasting) {
            return;
        }
        isCasting = true;
        GameObject spell = Instantiate( spellPrefab, spellSpawnPoint.position, spellSpawnPoint.rotation, spellSpawnPoint );
        currSpell = spell.GetComponent<Spell>();
        currSpell.Init();
    }

    void CastSpell() {
        currSpell.StopCast();
        isCasting = false;
        Ray ray = mainCamera.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );
        Vector3 target = Physics.Raycast( ray, out RaycastHit hit ) ? hit.point : ray.GetPoint( 50f );
        Vector3 direction = (target - spellSpawnPoint.position).normalized;

        currSpell.Cast( direction );
        currSpell = null;
    }

    void OnEnable() {
        castSpellAction.Enable();
    }
    void OnDisable() {
        castSpellAction.Disable();
    }
}
