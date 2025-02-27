using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("Thema")]
public class Thema                  //Klasse zum Speichern von Themen
{
    [XmlRoot("Frage")]
    public class Frage              //Klasse zum Speichern von Fragen
    {
        [XmlRoot("Antwort")]
        public class Antwort        //Klasse zum Speichern von Antworten
        {
            [XmlElement("AntwortText")]
            public string AntwortText;      //Der Text einer Antwortmöglichkeit, z.B. "Ja"
            [XmlElement("Next")]
            public string Next;             //die empfohlene nächste Frage, -> id der Frage z.B. "5", falls Themenwechsel "neuesThema/5", Sonderfälle möglich z.B. "Stop", "Abbruch", "Ende"
        }


        [XmlElement("ID")]
        public uint ID;             //ID einer Frage sollte in einem Thema möglichst eindeutig sein, immer eine positive Ganzzahl z.B. 5
        [XmlElement("Text")]
        public string Text;         //Der Text der Frage, die dem Patienten gestellt wird z.B. "Haben Sie schmerzen?"
        [XmlElement("Optional")]
        public bool Optional;       //bool der anzeigt, ob eine Antwort auf diese Frage erwartet wird
        [XmlElement("Next")]
        public string Next;         //Default empfohlene nächste Frage für alle Antworten (oder keine Antwort), -> id der Frage z.B. "5", falls Themenwechsel "neuesThema/5", Sonderfälle möglich z.B. "Stop", "Abbruch", "Ende"

        [XmlArray("Antworten"), XmlArrayItem("Antwort")]
        public List<Antwort> Antworten = new List<Antwort>();   //Array aller voergegebenen Antwortmöglichkeiten

        public void addAntwort(string antwort)      //Fürgt eine neue Antwortmöglichkeit hinzu (vermutlich nicht notwendig für diese Anwendung)
        {
            Antworten.Add(new Antwort());
            Antworten[Antworten.Count - 1].AntwortText = antwort;
        }

        public List<string> getAntworten()
        {
            List<string> returnAntworten = new List<string>();
            foreach (Antwort antwort in Antworten)
            {
                returnAntworten.Add(antwort.AntwortText);
                
            }
            return returnAntworten;
        }

    }


    [XmlElement("Titel")]
    public string Titel;                            //Der Titel des Themas

    [XmlArray("Fragen"), XmlArrayItem("Frage")]
    public List<Frage> Fragen = new List<Frage>();  //Array aller Fragen zu diesem Thema
    
    public string toString()
    {
        return "Titel: " + Titel + " Anzahl Fragen: " + Fragen.Count;
    }

    public void addFrage(string frage)              //Fügt eine neue Frage hinzu (vermutlich nicht notwendig für diese Anwendung)
    {
        Fragen.Add(new Frage());
        Fragen[Fragen.Count - 1].Text = frage;
    }

}
