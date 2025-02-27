//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class DialogSerializer : MonoBehaviour
{
    public DialogContainer dialogContainer;             //Container für den Ganzen Dialog (brauch ich den hier überhaupt?)

    public static DialogContainer Load(string path)     //Läd den dialog aus <path> und gibt einen DialogContainer zurück
    {
        DialogContainer container = new DialogContainer();
        var serializer = new XmlSerializer(typeof(DialogContainer));
        var stream = new FileStream(path, FileMode.Open);
        container = (DialogContainer)serializer.Deserialize(stream);
        stream.Close();
        Debug.Log("Dialog Opend");
        return container;
    }

    public static void Save(string path, DialogContainer dialogContainerToSave) //Speichert den DialogContainer als <path>
    {
        var serializer = new XmlSerializer(typeof(DialogContainer));

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, dialogContainerToSave);
        }
        Debug.Log("File Saved as "+ path);
    }
}
