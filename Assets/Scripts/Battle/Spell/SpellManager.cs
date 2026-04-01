using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour {
    [SerializeField] List<GameObject> spellPrefabs;
    [SerializeField] Transform spellSpawnPoint;
    [SerializeField] Camera mainCamera;
    [SerializeField] InputReader inputReader;
    [SerializeField] SpellMachine spellMachine;

    SpellUI spellUI;
    Spell currSpell;
    GameObject selectedSpell;

    SpellData spellData;
    SpellData nextSpellData;

    bool isCasting = false;

    void Awake() {
        spellUI = FindFirstObjectByType<SpellUI>();
    }

    void Start() {
        nextSpellData = spellMachine.GetRandomSpellData();
    }

    void OnEnable() {
        inputReader.OnStartCast += StartCast;
        inputReader.OnReleaseCast += ReleaseCast;
    }

    void OnDisable() {
        inputReader.OnStartCast -= StartCast;
        inputReader.OnReleaseCast -= ReleaseCast;
    }

    void AssignNextSpell() {
        spellData = nextSpellData;
        nextSpellData = spellMachine.GetRandomSpellData();
    }

    void StartCast() {
        if(isCasting || GameManager.i.CurrentGameState != GameStates.Playing) return;
        if(spellData == null) {
            AssignNextSpell();
            if(spellData == null) return;
        }

        isCasting = true;
        selectedSpell = GetPrefabByElement( spellData.element );

        GameObject spell = Instantiate( selectedSpell, spellSpawnPoint.position, spellSpawnPoint.rotation, spellSpawnPoint );
        currSpell = spell.GetComponent<Spell>();
        currSpell.Init();
        spellUI.SetSpellUI( currSpell.SpellType.SpellIcon );
        spellUI.SetNextSpellUI( nextSpellData != null ? nextSpellData.element : ElementID.None );
    }

    void ReleaseCast() {
        if(!isCasting || currSpell == null) return;

        currSpell.transform.SetParent( null );
        currSpell.StopCast();

        Vector3 direction = CalculateSpellDirection();
        currSpell.Cast( direction );

        spellMachine.ConsumeSpellData( spellData );

        AssignNextSpell();

        currSpell = null;
        isCasting = false;
    }

    Vector3 CalculateSpellDirection() {
        if(UnityEngine.XR.XRSettings.isDeviceActive) {
            return spellSpawnPoint.forward;
        } else {
            Ray ray = mainCamera.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );
            if(Physics.Raycast( ray, out RaycastHit hit )) {
                return (hit.point - spellSpawnPoint.position).normalized;
            }
            return mainCamera.transform.forward;
        }
    }

    GameObject GetPrefabByElement(ElementID element) {
        return spellPrefabs.Find( prefab => {
            Spell spellComponent = prefab.GetComponent<Spell>();
            return spellComponent != null && spellComponent.SpellType.Element == element;
        } );
    }
}
