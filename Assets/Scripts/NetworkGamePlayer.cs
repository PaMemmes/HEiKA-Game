using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

//Script zur verwaltung eines Spielers während des Spiels
public class NetworkGamePlayer : NetworkBehaviour
{
    [SyncVar]
    public string displayName = "Loading...";   //Name des Spielers
    [SyncVar]
    public string Role = "Loading...";          //Rolle des Spielers ("Patient", "Arzt", "Zuschauer")

    private NetworkManagerLobby room;           

    private NetworkManagerLobby Room            //wenn kein Room existiert, wird einder durch den Server erzeugt
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    //public override void OnStartClient()        //Verbindung wird hergestellt
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);

    }

    //public override void OnStopClient()         //Verbindung wird beendet
    public void Stop()
    {
        Room.GamePlayers.Remove(this);
    }

    [ClientRpc]
    public void RpcRemovePlayerAt(int playerIndex)
    {
        Room.GamePlayers.RemoveAt(playerIndex);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    [Server]
    public void SetRole(string role)
    {
        this.Role = role;
    }
}
