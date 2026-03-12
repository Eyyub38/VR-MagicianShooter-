using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellUI : MonoBehaviour {
    [SerializeField] List<Image> spellImages;
    [SerializeField] Color unSelectedColor;

    List<Color> originalColor;

    void Awake() {
        originalColor = new List<Color>();
        TakeOriginalColors();
    }

    void TakeOriginalColors() {
        for(int i = 0; i < spellImages.Count; i++) {
            originalColor.Add( spellImages[i].color );
        }
    }

    public void SetSpellUI(int index) {
        for(int i = 0; i < spellImages.Count; i++) {
            if(i != index) {
                spellImages[i].color = unSelectedColor;
            } else {
                spellImages[i].color = originalColor[i];
            }
        }
    }
}