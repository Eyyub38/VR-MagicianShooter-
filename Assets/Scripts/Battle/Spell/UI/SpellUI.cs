using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellUI : MonoBehaviour {
    [SerializeField] Image spellUIImage;
    [SerializeField] Image nextSpellUIImage;
    [SerializeField] Sprite noneSprite;


    public void SetSpellUI(Sprite spellIcon) {
        if(spellIcon == null) {
            spellUIImage.sprite = noneSprite;
            return;
        }
        spellUIImage.sprite = spellIcon;
    }

    public void SetNextSpellUI(ElementID element) {
        if(element == ElementID.None) {
            nextSpellUIImage.sprite = noneSprite;
            return;
        }
        SpellMachine.i.spellCache.TryGetValue( element, out SpellType spellType );
        nextSpellUIImage.sprite = spellType.SpellIcon;
    }
}
