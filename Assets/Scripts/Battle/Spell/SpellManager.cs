using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour {
    [SerializeField] List<GameObject> spellPrefabs;
    [SerializeField] Transform spellSpawnPoint;
    [SerializeField] Camera mainCamera;
    [SerializeField] InputReader inputReader;

    SpellUI spellUI;
    Spell currSpell;
    GameObject selectedSpell;

    bool isCasting = false;

    void Awake() {
        spellUI = FindFirstObjectByType<SpellUI>();
    }

    void Start() {
        SetSpell( 0 );
        spellUI.SetSpellUI( 0 );
    }

    void OnEnable() {
        inputReader.OnSpellSelected += HandleSpellSelection;
        inputReader.OnStartCast += StartCast;
        inputReader.OnReleaseCast += ReleaseCast;
    }

    void OnDisable() {
        inputReader.OnSpellSelected -= HandleSpellSelection;
        inputReader.OnStartCast -= StartCast;
        inputReader.OnReleaseCast -= ReleaseCast;
    }

    void StartCast() {
        if(isCasting || GameManager.i.CurrentGameState != GameStates.Playing) return;
        isCasting = true;
        GameObject spell = Instantiate( selectedSpell, spellSpawnPoint.position, spellSpawnPoint.rotation, spellSpawnPoint );
        currSpell = spell.GetComponent<Spell>();
        currSpell.Init();
    }

    void ReleaseCast() {
        if(!isCasting || currSpell == null) return;
        currSpell.transform.SetParent( null );

        currSpell.StopCast();
        isCasting = false;

        Vector3 direction = CalculateSpellDirection();
        currSpell.Cast( direction );
        currSpell = null;
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

    void HandleSpellSelection(int index) {
        if(index >= 0 && index < spellPrefabs.Count) {
            selectedSpell = spellPrefabs[index].gameObject;
            spellUI.SetSpellUI( index );
        }
    }
    void SetSpell(int index) {
        selectedSpell = spellPrefabs[index].gameObject;
    }

}
