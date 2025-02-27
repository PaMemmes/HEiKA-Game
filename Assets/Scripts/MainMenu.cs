using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script f�r das Hauptmenu das eigentlich nur einen neuen Server startet
public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null; //Verkn�pfung zum NetworkManager

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;        //Verkn�pfung zur Menu, ob man selbst Hosten will

    public void HostLobby()                 //Nutzer startet einen neuen Server
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);  //Menu wird geschlosssen
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
