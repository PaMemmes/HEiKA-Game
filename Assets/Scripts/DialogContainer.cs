using System.Xml;
using System.Xml.Serialization;

using System.Collections.Generic;
using UnityEngine;

[XmlRoot("Dialog")]
public class DialogContainer                            //Container für den ganzen Dialog
{
    
    [XmlArray("Themen"), XmlArrayItem("Thema")]
    public List<Thema> Themen = new List<Thema>();      //Hier werden allte themen als Array gespeichert

    public List<Thema> getThemen()                      //gibt alle Themen zurück
    {
        return Themen;
    }

    public Thema addThema(string titel)                 //Fügt ein neues Thema mit titel <titel> hinzu (wird für diese Anwendung vermutlich nicht gebraucht)
    {
        Themen.Add(new Thema());
        Themen[Themen.Count - 1].Titel = titel;
        return Themen[Themen.Count - 1];
    }

    public void addThema(Thema thema)                 
    {
        Themen.Add(thema);
        Debug.Log("AddingThema " + thema.Titel);
    }
}
