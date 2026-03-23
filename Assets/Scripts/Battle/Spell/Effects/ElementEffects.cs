using UnityEngine;

enum ElementsType { None, Fire, Ice, Lightning, Wet, Shocked }

public class ElementEffects {
    ElementsType type;
    float duration;
}

[CreateAssetMenu( fileName = "New Element Data", menuName = "Spell/Element Data" )]
public class ElementData : ScriptableObject {
    public string elementName;
    public Color color;
}

[CreateAssetMenu( fileName = "New Element Data", menuName = "Spell/Element Reaction" )]
public class ElementReaction : ScriptableObject {
    public ElementData element1;
    public ElementData element2;
    public ElementData result;
    public float damageMultiplier = 1f;
}

