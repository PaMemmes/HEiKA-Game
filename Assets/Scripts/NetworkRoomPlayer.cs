using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

/*Script f�r die Einzelnen Spieler in der Lobby
 hier k�nnen Spieler ihre Rolle und ihre Bereitschaft �ndern*/
public class NetworkRoomPlayer : NetworkBehaviour
{
    [Header("UI")]                                                          //Verkn�pfungen zur UI
    [SerializeField] private GameObject lobbyUI = null;                     //Lobby an sich
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];  //Felder f�r die Spielernamen
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4]; //Felder f�r die Spielerbereitschaft
    [SerializeField] private TMP_Text[] playerRoleTexts = new TMP_Text[4];  //Felder f�r die Spielerrollen
    [SerializeField] private Button startGameButton = null;                 //Start Button
    //[SerializeField] private TMP_Text ipAdress = null;                      //Anzeige der IP-Adresse

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";                               //Default Name
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;                                            //Default Ready
    [SyncVar(hook = nameof(HandleDisplayRoleChanged))]
    public string Role = null;                                              //Default Role

    
    private bool isLeader;

    public bool IsLeader    //Wird Aufgerufen, falls der Spieler Der Host ist -> Aktiviert IP anzeige und Start-Button
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
            /* F�r bei WebGL zu Problemen
            if (value) {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAdress.text = "IP Adresse: " + ip.ToString();
                    }
                }
            }*/


        }
    }

    private NetworkManagerLobby room;

    private NetworkManagerLobby Room        //Erzeugt einen Room, falls keiner verf�gbar
    {
        get
        {
            
            if (room != null) { return room; }
            Debug.Log("-----------------------Client creating new room-------------------- -");
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

    //public override void OnStartClient()
    public void Start()
    {
        Debug.Log("-------OnStartClient - Room.RoomPlayers.Add ---------");
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    //public override void OnStopClient()
    public void Stop()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }

    //Funktionen, faslls sich Name, Status, Rolle �ndern
    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    public void HandleDisplayRoleChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()        //Wird aufgerufen, fals sicht die Anzeige ge�ndert hat (z.B. Ein Spieler hat die Rolle gewechselt)
    {
        Debug.Log("UpdateDisplay: RoomPlayers.Count = " + Room.RoomPlayers.Count + ", GamePlayers.Count = " + Room.GamePlayers.Count);
        if (!isOwned)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isOwned)
                {
                    player.UpdateDisplay();                     //Ruft Update Display bei allwn andern Spielern auf
                    break;
                }
            }
            return;
        }

        for (int i = 0; i< playerNameTexts.Length; i++)         //Kein Spieler da
        {
            Debug.Log("Ich habe eine Anzeige auf null gesetzt");
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
            playerRoleTexts[i].text = string.Empty;
        }

        for (int i = 0; i< Room.RoomPlayers.Count; i++)         //Anzeige wird aktualisiert
        {
            Debug.Log("Der " + (i + 1) + ". Spieler wird initialisiert");
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
            playerRoleTexts[i].text = Room.RoomPlayers[i].Role;
        }


    }

    [ClientRpc]
    public void RpcRemovePlayerAt(int playerIndex)      //Teilt einem Spieler mit das ein anderer Spieler den Server Verlassen hat
    {
        Room.RoomPlayers.RemoveAt(playerIndex);
    }

    [ClientRpc]
    public void RpcUpdateDisplay()                      //Sagt einem Spieler, dass die UI geupdatet werden soll.
    {
        UpdateDisplay();
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        //if (isLeader) { return; }
        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
        //ipAdress.text = room.networkAddress;
    }

    [Command]
    public void CmdPatient()
    {
        Role = "Patient";
    }

    [Command]
    public void CmdArzt()
    {
        Role = "Arzt";
    }

    [Command]
    public void CmdSpectator()
    {
        Role = "Zuschauer";
    }

    [Command]
    public void CmdStartGame()      //Spieler sagt dem Server, das das Spiel beginnen soll
    {
        //if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }
        Room.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
