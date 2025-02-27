using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//behälfsmäßiges Script zur Verwaltung von Objekten, mit denen man interagieren kann
public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;       //Event, welches durch das Interagieren ausgelöst wird
    public Sprite interactIcon;         //Icon, zu dem der Kurser wechselt, wenn das Object betrachtet wird
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
        ID = Random.Range(0, 999999);   //TODO: Theoretisch unpracktisch für viele Interactables, Chance für Probleme extrem gering 
    }
}