using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Script zum Anzeigen der Überwachungskammera in der Arzt UI
 * Wenn 1 geöffnet wird, wird die anzeige an/aus geschaltet*/


public class ArztCamerasScript : MonoBehaviour
{
    /* wird nicht mehr verwendet
    [SerializeField] private RawImage cameraFront = null;   //Verknüpfung zum UI-Element mit der Kamera Textur
    private bool cameraFrontEnabled = false;                //Speichert, ob die Textur aktiviert ist

    void Start()
    {
        cameraFront.enabled = cameraFrontEnabled;           //cameraFront startet deaktiviert (cameraFrontEnabled = false)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))               //Wenn 1 gedrückt wird, wird cameraFrontEnabled gewechselt und cameraFront.enabled wird angepasst
        {
            cameraFrontEnabled = !cameraFrontEnabled;
            cameraFront.enabled = cameraFrontEnabled;
        }
        //TODO? Hier können weitere Cameras eingabeut werden
    }*/
}
