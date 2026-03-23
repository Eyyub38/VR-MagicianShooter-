using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class EnemyElementProcessor : MonoBehaviour {
    [SerializeField] float elementDuration = 5f;
    List<ActiveElement> activeElements = new List<ActiveElement>();
    List<ElementReaction> elementReactions = new List<ElementReaction>();

    public void OnHit(ElementData newElement) {
        if(newElement == null) return;

        foreach(var reaction in elementReactions) {
            ActiveElement matchA = activeElements.Find( e => e.elementData == reaction.element1 );
            ActiveElement matchB = activeElements.Find( e => e.elementData == reaction.element2 );

            if((matchA != null && newElement == reaction.element2) ||
                (matchB != null && newElement == reaction.element1)) {

                TriggerReaction( reaction );
                return;
            }
        }

        ActiveElement active = activeElements.Find( e => e.elementData == newElement );
        if(active != null) {
            StopCoroutine( active.elementTimer );
            active.elementTimer = StartCoroutine( ElementTimer( newElement ) );
        } else {
            Coroutine timer = StartCoroutine( ElementTimer( newElement ) );
            activeElements.Add( new ActiveElement( newElement, timer ) );
        }
    }

    IEnumerator ElementTimer(ElementData element) {
        yield return new WaitForSeconds( elementDuration );

        ActiveElement toRemove = activeElements.Find( e => e.elementData == element );
        if(toRemove != null) {
            activeElements.Remove( toRemove );
            Debug.Log( element.elementName + " etkisi sona erdi." );
        }
    }

    public void TriggerReaction(ElementReaction reaction) {
        RemoveActiveElement( reaction.element1 );
        RemoveActiveElement( reaction.element2 );

        if(reaction.result != null) {
            OnHit( reaction.result );
        }
    }

    void RemoveActiveElement(ElementData data) {
        ActiveElement element = activeElements.Find( e => e.elementData == data );
        if(element != null) {
            StopCoroutine( element.elementTimer );
            activeElements.Remove( element );
        }
    }
}

[System.Serializable]
public class ActiveElement {
    public ElementData elementData;
    public Coroutine elementTimer;

    public ActiveElement(ElementData data, Coroutine timer) {
        this.elementData = data;
        this.elementTimer = timer;
    }
}