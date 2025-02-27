using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Verwaltung von Events zum �ffnen und schlie�en des Chats (dient der Kommunikation der verschiedenen Scripte)

public class MainSceneEventManager : MonoBehaviour
{
    public delegate void OpenChat();
    public delegate void CloseChat();

    public static event OpenChat OnOpenChat;    //Event wenn der Chat ge�ffnet wird
    public static event CloseChat OnCloseChat;  //Event wenn der Chat geschlossen wird

    public Canvas pauseMenuCanvas; // Assign your pause menu Canvas here
    public string pauseMenuCanvasName = "Canvas_MainMenu";
    private bool isPaused = false;

    void Start() 
    {
        GameObject canvasObject = GameObject.Find(pauseMenuCanvasName);
        if (canvasObject != null)
        {
            pauseMenuCanvas = canvasObject.GetComponent<Canvas>();
            canvasObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Canvas with name '" + pauseMenuCanvasName + "' not found!");
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        pauseMenuCanvas.gameObject.SetActive(true); // Show pause menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.gameObject.SetActive(false); // Hide pause menu
        Cursor.visible = false; // Hide cursor
        CloseChatTrigger();
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }




    public static void OpenChatTrigger() //Chat wird ge�ffnet (und z.B. Bewegung im MovementManager deaktiviert)
    {
        if (OnOpenChat != null)
        {
            OnOpenChat();
        }
    }

    public static void CloseChatTrigger() //Chat wird geschlossen
    {
        if (OnCloseChat != null)
        {
            OnCloseChat();
        }
    }
    
}
