using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemaButtonScript : MonoBehaviour
{
    /*
    [SerializeField] public Text buttonText;

    public Thema thema;
    [SerializeField] GameObject FragenPref;
    [SerializeField] public Transform ScrollViewContent;
    public ChatBehaviour Chat;
    //public DialogEditor dialogEditor;

    public void ReLoadFragen()
    {
        //ThemenTitel.text = thema.Titel.Trim();
        deleteFragenButtons();
        createFragenButtons();
    }

    public void createFragenButtons()
    {
        int fragenCount = thema.Fragen.Count;
        Debug.Log("fragenCount: " + fragenCount);
        for (int i = 0; i < fragenCount; i++)
        {
            Debug.Log("i: " + i);
            Debug.Log("Creating FragenButton " + thema.Fragen[i].Text);
            GameObject FragenButton = Instantiate(FragenPref, ScrollViewContent) as GameObject;
            FragenButton.GetComponent<FragenButtonScript>().setFrage(thema.Fragen[i]);
            //FragenButton.GetComponent<FragenScript>().ScrollViewContent = AntwortenScrollViewContent;
            //FragenButton.GetComponent<FragenScript>().FragenTitel = FragenTitel;
            //FragenButton.GetComponent<FragenScript>().dialogEditor = dialogEditor;
            FragenButton.gameObject.transform.GetComponent<Button>().onClick.AddListener(delegate { askQuestion(FragenButton.GetComponent<FragenButtonScript>().frage); });
            
        }

    }

    public void deleteFragenButtons()
    {
        int childs = ScrollViewContent.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(ScrollViewContent.transform.GetChild(i).gameObject);
        }
    }

    public void setThema(Thema newThema) //ref?
    {
        thema = new Thema();
        thema = newThema;
        buttonText.text = thema.Titel.Trim();
    }

    public void askQuestion(Thema.Frage frage)
    {
        Chat.AskQuestion(frage.Text.Trim(),frage.getAntworten());
        // TODO: Fertige Antwortmöglichkeiten an den Patienten senden
    }*/
}
