using System;
using UnityEngine;

public enum Element {}

public abstract class SpellEffect : ScriptableObject {
    public abstract void Apply(GameObject target);
}

