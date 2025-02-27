using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Script f�r den Server
 * Beginnt in der Lobby, hier joinen Spieler, w�hlen ihre Rolle, etc...
 */
public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 1;                        //minimale Spieleranzahl um das Spiel zu beginnen
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = null; //SpielerPrefab in der Lobby

    [Header("Game")]
    [SerializeField] private NetworkGamePlayer gamePlayerPrefab = null; //SpielerPrefab im Spiel
                                                                        //[SerializeField] private GameObject playerSpawnSystem = null;

    public static event Action OnClientConnected;                   //Event, wenn ein Spieler beitritt
    public static event Action OnClientDisconnected;                //Event, wenn ein Spieler disconnected
    //public static event Action OnStartClient;
    //public static event Action OnStopClient;

    public static event Action<NetworkConnection> OnServerReadied;

    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();    //Liste der Spieler in der Lobby
    public List<NetworkGamePlayer> GamePlayers { get; } = new List<NetworkGamePlayer>();    //Liste der Spieler im Spiel

    [SerializeField] private GameObject spectatorPrefab = null;         //Prefabs f�r die einzelnen Rollen
    [SerializeField] private GameObject patientPrefab = null;
    [SerializeField] private GameObject arztPrefab = null;

    [SerializeField] private ArztFragenScript_v2 FragenScript = null;



    public override void OnStartServer()                    //Wird bei Serverstart ausgef�hrt
    {
        Debug.Log("---------------------------OnStartServer-------------------------");
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();  //Prefabs werden bereit geamcht
        FragenScript.LoadQuestions();
    }


    public override void OnStartClient()                    //Wird beim Starten eines Clients ausgef�hrt
    {
        Debug.Log("---------------------------OnStartClient-------------------------");
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");   //Prefabs werden bereit geamcht

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    //public override void OnClientConnect(NetworkConnection conn)
    public override void OnClientConnect()
    {
        //base.OnClientConnect(conn);
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    //public override void OnClientDisconnect(NetworkConnection conn)
    public override void OnClientDisconnect()
    {
        //base.OnClientDisconnect(conn);
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" != menuScene) //Stopps joining midgame
        {
            conn.Disconnect();
            return;
        }
        
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if ("Assets/Scenes/"+SceneManager.GetActiveScene().name+".unity" == menuScene)
        {
            Debug.Log("-------OnServerAddPlayer - True ---------");
            //bool isLeader = RoomPlayers.Count == 0;
            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);
            //roomPlayerInstance.IsLeader = isLeader;
            if (RoomPlayers.Count == 0)
            {
                roomPlayerInstance.Role = "Arzt";
            }
            if (RoomPlayers.Count == 1)
            {
                roomPlayerInstance.Role = "Patient";
            }
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            
        }
        else
        {
            Debug.Log("-------OnServerAddPlayer - False ---------");
        }
        
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)             //Wird auf dem Server aufgerufen, wenn ein Spieler den Server verl�sst -> Spielerliste mus ge�ndert werden
    {
        if (conn.identity != null)
        {
            var roomPlayer = conn.identity.GetComponent<NetworkRoomPlayer>();
            if (roomPlayer != null)                                                 //Der spieler war in der Lobby
            {
                int roomPlayerIndex = RoomPlayers.IndexOf(roomPlayer);
                RoomPlayers.Remove(roomPlayer);
                foreach (var p in RoomPlayers)
                {
                    p.RpcRemovePlayerAt(roomPlayerIndex);
                    p.RpcUpdateDisplay();
                }

                NotifyPlayersOfReadyState();
            }
            else
            {
                var gamePlayer = conn.identity.GetComponent<NetworkGamePlayer>();   //Der spieler war in einem laufendem Spiel
                int gamePlayerIndex = GamePlayers.IndexOf(gamePlayer);
                GamePlayers.Remove(gamePlayer);
                foreach (var p in GamePlayers)
                {
                    p.RpcRemovePlayerAt(gamePlayerIndex);
                }
            }
            
        }
        base.OnServerDisconnect(conn);
        Debug.Log("RoomPlayers.Count = "+ RoomPlayers.Count+ ", GamePlayers.Count = " + GamePlayers.Count);
        if (RoomPlayers.Count == 0 & GamePlayers.Count == 0)                        //Der Server wechselt zur�ck in die Lobby, falls keine Spieler mehr im spiel sind.
        {
            Debug.Log("Server sollte jetzt restarten");
            base.ServerChangeScene(onlineScene);
        }
        
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()     //Ein Spieler �ndert, ob er bereit ist
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }


    private bool IsReadyToStart()           //Testet ob alle Spieler bereit sind
    {
        if (numPlayers < minPlayers)
        {
            return false; 
        }
        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }
        return true;
    }

    public void StartGame()                 //Startet das Spiel, wenn alle Spieler bereit sind
    {
        if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
        {
            if (!IsReadyToStart())
            {
                return;
            }
            ServerChangeScene("MainScene");
        }
    }

    public override void ServerChangeScene(string newSceneName)     //Server wechselt die Scene, aktuell nur zur MainScene (das eigentliche Spiel)
    {
        // From Menu to Game
        if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
        {
            for (int i = RoomPlayers.Count -1; i>= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                gameplayerInstance.SetRole(RoomPlayers[i].Role);
                RoomPlayers.RemoveAt(i);                            //Der Spieler wird aus der Liste der RoomPlayer entfernt
                foreach (var p in RoomPlayers)
                {
                    p.RpcRemovePlayerAt(i);
                }
                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
                
            }
        }
        base.ServerChangeScene(newSceneName);
        //TODO: From Game to Menu
    }

    
    public override void OnServerSceneChanged(string sceneName) //Szene wurde Gewechselt
    {
        spawnPlayers();                                         //Szene wird mit Spielern gef�llt
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
        
    }


    public void spawnPlayers()                              //Initialisiert die Spieler Passend zu ihrer Rolle in der Spielwelt
    {
        Debug.Log("------------Spawning Players - RoomPlayer: " + RoomPlayers.Count + " GamePlayer: " + GamePlayers.Count);
        for (int i = GamePlayers.Count - 1; i >= 0; i--)
        {
            Debug.Log("---------------spawnPlayers i = "+i+"-----------------");
            var conn = GamePlayers[i].connectionToClient;
            Debug.Log("---------------spawnPlayers role = " + GamePlayers[i].Role + " Name: " + GamePlayers[i].displayName +"-----------------");
            if (GamePlayers[i].Role == "Patient")           //Der Spieler will Patient sein
            {
                GameObject patientInstance = Instantiate(patientPrefab);
                ChatBehaviour chatBehaviour = patientInstance.GetComponent<ChatBehaviour>();
                chatBehaviour.setName(GamePlayers[i].displayName);
                NetworkServer.Spawn(patientInstance, conn);
            }
            if (GamePlayers[i].Role == "Arzt")              //Der Spieler will Arzt sein
            {
                GameObject arztInstance = Instantiate(arztPrefab);
                ChatBehaviour chatBehaviour = arztInstance.GetComponent<ChatBehaviour>();
                chatBehaviour.setName(GamePlayers[i].displayName);
                
                
                NetworkServer.Spawn(arztInstance, conn);
                /*
                int j = 0;
                foreach (Thema thema in FragenScript.dialogContainer.getThemen())
                {
                    FragenScript.TargetSendThemaClientRpc(thema);
                    FragenScript.TargetSendIntClientRpc(j);
                    FragenScript.Print(j);
                    FragenScript.PrintClientRpc(j);
                    j++;
                }
                FragenScript.createThemenButtonsClientRpc();*/
                Debug.Log("Finished Spawning Arzt");

            }
            if (GamePlayers[i].Role == "Zuschauer")         //Der Spieler will Zuschauer sein
            {
                GameObject spectatorInstance = Instantiate(spectatorPrefab);
                NetworkServer.Spawn(spectatorInstance, conn);

            }
        }
    }

}
