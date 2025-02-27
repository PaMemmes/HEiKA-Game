using System.Collections;
using System.Collections.Generic;
using Inputs;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
using Unity.Cinemachine;
using Mirror;
using TMPro;

/*Script um die Camerasteuerung zu verwalten. 
 * Aktuell auch noch f�r aktivieren und Deaktivieren verschiedener UI-Elemente zust�ndig 
 * TODO: in Einfachere Scripte unterteilen*/
public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private GameObject head = null;              //Kopfposition
    [SerializeField] private Transform playerFaceTransform = null;              //Kopfposition
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;     //Verkn�pfung zur Kamera

    [Header("Chat")] //Verkn�pfungen zur Chat UI
    [SerializeField] private TMP_InputField chatEingabe = null;                 //Eingabefeld f�r neue Nachrichten
    [SerializeField] private Button chatCloseButton = null;
    
    [SerializeField] private Button nxtButton = null;                     //Button zum schlie�en des Chats
    [SerializeField] private GameObject ScrollView = null;
    [SerializeField] private TMP_Text chatText = null;                          //Textfeld f�r den Chat
    private bool Typing = false;                                                //schreibt der arzt gerade im Chat? -> blockiert schlie�en des Chats mit "e"

    [Header("Role")]
    [SerializeField] private string Role = null;                                //Rolle des Spielers -> Verschiedene Aktionen f�r verschiedene Rolle -> TODO: in verschiedene Scripte unterteilen

    [Header("Interaction")]
    public LayerMask interactablelayermask;                                     //Maske f�r alle Objecte, mit denen der Patient Interagieren kann
    Interactable interactable;
    public Image interactImage;                                                 //Icon fur das angesehene Interactable
    public Sprite defaultIcon;                                                  //Default Icon falls kein Interactable angesehen wird
    public Sprite defaultInteractIcon;                                          //Default Icon falls ein Interactable ohne eigenes Icon angesehen wird
    public Sprite emptyTexture;                                                 //leerer Curser (falls Chat ge�ffnet)
    private bool active = false;

    private PlayerMovementController movementController = null;                 //Verkn�pfung zum MovementController des Spielers

    
    private Controls controls;      //m�glicherweise nicht mehr benutzt/unn�tig
    private Controls Controls       //Erzeugt neue Controlls, fals keine vorhanden
    {
        get
        {
            if(controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    private CinemachineFollow transposer;

    public override void OnStartAuthority()                                     //Der eigene CameraController wird gestartet
    {
        movementController = gameObject.GetComponent(typeof(PlayerMovementController)) as PlayerMovementController; //Werte werden Initailiert
        CinemachineFollow transposer = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFollow;
        virtualCamera.gameObject.SetActive(true);
        enabled = true;
        active = true;
        Cursor.lockState = CursorLockMode.Locked;
        if(Role == "Zuschauer")
        {
            chatText.enabled = false;
        }
        if (Role == "Arzt")
        {
            chatEingabe.enabled = false;
        }
        if (head != null)
        {
            head.SetActive(false);
        }

    }


    //Verschiedene Funktionen um Camerabewegung und normale Bewegung f�r die Verschiedenen Rollen zusammen mit passenden UI-Elementen zu aktivieren und Deakivieren
    [Client]
    public void disableCameraMovementPatient()
    {
        active = false;
        Cursor.lockState = CursorLockMode.None;
        chatEingabe.gameObject.SetActive(true);
        chatCloseButton.gameObject.SetActive(true);
        nxtButton.gameObject.SetActive(true);
        ScrollView.gameObject.SetActive(true);
        Cursor.visible = true;
        virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim).enabled = false;
        
       

    }

    [Client]
    public void enableCameraMovementPatient()
    {
        active = true;
        Cursor.lockState = CursorLockMode.Locked;
        chatEingabe.gameObject.SetActive(false);
        chatCloseButton.gameObject.SetActive(false);
        nxtButton.gameObject.SetActive(false);
        ScrollView.gameObject.SetActive(false);
        Cursor.visible = false;
        virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim).enabled = true;

    }

    [Client]
    public void disableCameraMovementArzt()
    {
        active = false;
        Cursor.lockState = CursorLockMode.None;
        chatEingabe.gameObject.SetActive(true);
        Cursor.visible = true;
        virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim).enabled = false;


    }

    [Client]
    public void enableCameraMovementArzt()
    {
        active = true;
        Cursor.lockState = CursorLockMode.Locked;
        chatEingabe.gameObject.SetActive(false);
        Cursor.visible = false;
        virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Aim).enabled = true;
    }


    [Client]
    public void disablePlayerMovement()
    {
        movementController.stop();
    }

    [Client]
    public void enablePlayerMovement()
    {
        movementController.start();
    }

    [Client]
    public void setTyping(bool typing)
    {
        Typing = typing;
    }

    //Wird zu jedem Frame aufgerufen. Acuhtet daruf ob passende Tasten gedr�ckt werden, die den Chat aktivieren/deaktivieren
    [ClientCallback]
    void Update()
    {
        if (true)
        {
            if (Role == "Patient")
            {
                if (active)
                {
                    if (playerFaceTransform != null)
                    {
                        Quaternion orientation = virtualCamera.State.GetFinalOrientation();                             //Spielermodell schaut in richtung Camera
                        playerFaceTransform.rotation = Quaternion.Euler(orientation.x, orientation.y, -orientation.z);      //...
                    }
                    

                    //Testet ob der Patient ein Object zum Interagieren auschaut
                    RaycastHit hit;
                    if (Physics.Raycast(virtualCamera.transform.position, virtualCamera.transform.forward, out hit, 5, interactablelayermask))
                    {
                        if (hit.collider.GetComponent<Interactable>() != false)
                        {
                            if (interactable == null || interactable.ID != hit.collider.GetComponent<Interactable>().ID)
                            {
                                interactable = hit.collider.GetComponent<Interactable>();
                            }
                            if (interactable.interactIcon != null)         //passt das Icon an
                            {
                                interactImage.sprite = interactable.interactIcon;
                            }
                            else
                            {
                                interactImage.sprite = defaultInteractIcon;
                            }
                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                interactable.onInteract.Invoke();       //Ruft die spezifische Funktion des Objectes auf, mit dem interagiert wird
                            }
                        }
                    }
                    else
                    {
                        interactImage.sprite = defaultIcon;
                    }
                }
                else
                {
                    interactImage.sprite = emptyTexture;
                }
            }
            if (Role == "Zuschauer")
            {
                Quaternion orientation = virtualCamera.State.GetFinalOrientation();                         //Spielermodell schaut in richtung Camera
                playerFaceTransform.rotation = Quaternion.Euler(orientation.x, orientation.y, -orientation.z);  //...

                if (Input.GetKeyDown(KeyCode.E))        //e durch einen Zuschauer gedr�ckt -> offnet/schlie�t chatText
                {
                    if (chatText.enabled)
                    {
                        chatText.enabled = false;
                    }
                    else
                    {
                        chatText.enabled = true;
                    }
                }
            }
            if (Role == "Arzt")
            {
                Quaternion orientation = virtualCamera.State.GetFinalOrientation();                         //Spielermodell schaut in richtung Camera
                playerFaceTransform.rotation = Quaternion.Euler(orientation.x, orientation.y, -orientation.z);  //...

                if (Input.GetKeyDown(KeyCode.E))        //e durch einen Arzt gedr�ckt -> offnet/schlie�t Chat, Stopt/Startet Camerabewegung und Bewegung im Raum
                {
                    if (chatEingabe.enabled && (Typing == false))
                    {
                        chatEingabe.enabled = false;
                        enableCameraMovementArzt();
                        enablePlayerMovement();
                    }
                    else
                    {
                        chatEingabe.enabled = true;
                        disableCameraMovementArzt();
                        disablePlayerMovement();
                    }
                }
            }
        }
        
    }

    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();
        MainSceneEventManager.OnOpenChat += disableCameraMovementPatient;
        MainSceneEventManager.OnCloseChat += enableCameraMovementPatient;
    }

    [ClientCallback]
    private void OnDisable()
    {
        Controls.Disable();
        MainSceneEventManager.OnOpenChat -= disableCameraMovementPatient;
        MainSceneEventManager.OnCloseChat -= enableCameraMovementPatient;
    }
}
