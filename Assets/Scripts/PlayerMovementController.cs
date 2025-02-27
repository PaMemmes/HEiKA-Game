using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using Mirror;


//Script um Bewegung im Raum zu steuern
public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5f;              //Bewegungsgeschwindigkeit
    [SerializeField] private CharacterController controller = null; //Object das Bewegt wird
    [SerializeField] private Transform face = null;                 //Blickrichtung des Objectes

    private float defaultSpeed;

    private Vector2 previousInput;      //Aktuelle Bewegung (Horizontal)
    private float previousFlyInput;     //Aktuelle Bewegung (Vertikal)

    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    [Client]
    public void stop()
    {
        movementSpeed = 0f;
    }

    [Client]
    public void start()
    {
        movementSpeed = defaultSpeed;
    }


    public override void OnStartAuthority()
    {
        defaultSpeed = movementSpeed;
        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>()); //Steuerung für horizontale Bewegung wir initailisiert
        Controls.Player.Move.canceled += ctx => ResetMovement();

        Controls.Player.Fly.performed += ctx2 => SetFly(ctx2.ReadValue<float>());       //Steuerung für vertikale Bewegung wir initailisiert
        Controls.Player.Fly.canceled += ctx2 => ResetFly();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisable() => Controls.Disable();
    [ClientCallback]
    private void Update() => Move(); //Movent wird vom Spieler aus geregelt. -> Sollte Gefahr durch "Cheater" ein Problem sein ist das nicht sinvoll, so ist aber einfacher

    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]
    private void SetFly(float fly)
    {
        previousFlyInput = fly;
    }

    [Client]
    private void ResetFly() => previousFlyInput = 0;

    [Client]
    private void Move() //Wird durch Update() jeden Frame aufgerufen. BEwegt das Object passend zur aktuellen Bewegungseingabe
    {
        Vector3 up = new Vector3(0f, 1, 0f);       //Defult Vectoren, die mit der Bewegungsrichtung multipliziert werden
        Vector3 right = face.right;
        Vector3 forward = face.forward;
        right.y = 0f;
        forward.y = 0f;
        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y + up* previousFlyInput;    //movement wird passend zur Eingabe aktualisiert
        controller.Move(movement * movementSpeed * Time.deltaTime);                                                             //Spieler bewegt sich
    }
}
