using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class ArztFragenScript_v2 : NetworkBehaviour
{
    //[SyncVar]
    public DialogContainer dialogContainer = new DialogContainer();
    public ThemaButtonScript activeThemaButtonScript;

    [SerializeField] GameObject ThemaPref;
    [SerializeField] Transform ThemaScrollViewContent;
    [SerializeField] Transform FragenScrollViewContent;
    [SerializeField] ChatBehaviour Chat;

    [SerializeField] TMP_Dropdown ThemenDropdown;
    [SerializeField] GameObject FragenPref;


    [Server]
    public void LoadQuestions()         //L�d fragen
    {
        Debug.Log("Loading Questions");
        dialogContainer = new DialogContainer();
        dialogContainer = DialogSerializer.Load("Fragen.xml");
        if (dialogContainer == null)
        {
            Debug.LogError("Error: dialogContainer equals NULL");
        }
        if (dialogContainer.Themen.Count == 0)
        {
            Debug.Log("Warning: Dialog empty");
        }
    }

    public TMP_Dropdown GetThemenDropdown() 
    {
        return ThemenDropdown;
    }

    [Server]
    public override void OnStartServer()
    {
        LoadQuestions();
    }

    [Client]
    public override void OnStartClient()
    {
        CmdGetFragen();
        
    }



    public void SetDialogContainer(DialogContainer newDialogContainer)
    {
        dialogContainer = newDialogContainer;
        createThemenDropdown();
    }

    public void addThema(Thema thema)
    {
        dialogContainer.addThema(thema);
    }

    /* F�r alte Scrollview Auswahl
    public void createThemenButtons()                //Erzeugt die Buttons im UI
    {
        int themenCount = dialogContainer.Themen.Count;
        for (int i = 0; i < themenCount; i++)
        {
            GameObject ThemenButton = Instantiate(ThemaPref, ThemaScrollViewContent) as GameObject;
            ThemenButton.GetComponent<ThemaButtonScript>().setThema(dialogContainer.Themen[i]); //ref?
            ThemenButton.GetComponent<ThemaButtonScript>().ScrollViewContent = FragenScrollViewContent;
            ThemenButton.GetComponent<ThemaButtonScript>().Chat = Chat;

            ThemenButton.gameObject.transform.GetComponent<Button>().onClick.AddListener(delegate { setActiveThema(ThemenButton.GetComponent<ThemaButtonScript>()); });

        }
        Debug.Log("CreatedThemenButtons");

    }*/



    public void createThemenDropdown()
    {
        List<string> options = new List<string>();
        foreach (Thema thema in dialogContainer.getThemen())
        {
            options.Add(thema.Titel);
        }
        ThemenDropdown.AddOptions(options);
        createFragenButtons(0);
    }

    public void createFragenButtons(int ThemaID)
    {
        //R�umt alte Fragen auf
        int childs = FragenScrollViewContent.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(FragenScrollViewContent.transform.GetChild(i).gameObject);
        }

        //Erstellt neue Fragen
        Thema thema = dialogContainer.Themen[ThemaID];
        int fragenCount = thema.Fragen.Count;
        for (int i = 0; i < fragenCount; i++)
        {
            GameObject FragenButton = Instantiate(FragenPref, FragenScrollViewContent) as GameObject;
            FragenButton.GetComponent<FragenButtonScript>().setFrage(thema.Fragen[i]);
            FragenButton.gameObject.transform.GetComponent<Button>().onClick.AddListener(delegate { askQuestion(FragenButton.GetComponent<FragenButtonScript>().frage); });
        }
    }

    public void askQuestion(Thema.Frage frage)
    {
        List<string> antworten = new List<string>();
        foreach(string antwort in frage.getAntworten())
        {
            antworten.Add(antwort.Trim());
        }
        Chat.AskQuestion(frage.Text.Trim(), antworten);
    }

    [ClientRpc]
    public void createThemenButtonsClientRpc()
    {
        if (!isOwned) { return; }
        createThemenDropdown();
    }

    /*
    void setActiveThema(ThemaButtonScript newActiveThemaButton)
    {
        activeThemaButtonScript = newActiveThemaButton;
        activeThemaButtonScript.ReLoadFragen();
    }*/

    [Command]
    private void CmdGetFragen()
    {
        PrintClientRpc(10001);
        Print(10002);
        int j = 0;
        foreach (Thema thema in dialogContainer.getThemen())
        {
            PrintClientRpc(j);
            Print(j);
            SendThemaClientRpc(thema);
            j++;
        }
        createThemenButtonsClientRpc();
    }

    [ClientRpc]
    public void SendThemaClientRpc(Thema thema)
    {
        if (!isOwned) { return; }
        addThema(thema);
        Debug.Log("Recived Thema " + thema.Titel);
    }

    [ClientRpc]
    public void TargetSendIntClientRpc(int Nummer)
    {
        if (!isOwned) { 
            Debug.Log("Not Owner Recived Number " + Nummer);
            return;
        }
        Debug.Log("Recived Number " + Nummer);
    }

    [ClientRpc]
    public void PrintClientRpc(int Nummer)
    {
        Debug.Log("Print ClientRpc Number " + Nummer);
    }

    public void Print(int Nummer)
    {
        Debug.Log("Print Number " + Nummer);
    }
}
