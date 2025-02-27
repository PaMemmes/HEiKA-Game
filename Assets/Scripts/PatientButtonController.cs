using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PatientButtonController : NetworkBehaviour
{
    public Button nextQuestionButton;
    private AutoPilotScript autoPilotScript;

    void Start()
    {
        if (nextQuestionButton != null)
        {
            nextQuestionButton.onClick.AddListener(OnNextQuestionButtonClicked);
        }
    }

    [Command] // Command to send button press to the server
    void OnNextQuestionButtonClicked()
    {
        Debug.Log("Next Question button clicked by Patient.");
        AutoPilotScript autoPilotScript = FindObjectOfType<AutoPilotScript>();

        if (autoPilotScript != null)
        {
            autoPilotScript.TriggerNextQuestion();
        }
        else
        {
            Debug.LogWarning("AutoPilotScript not found on server!");
        }
    }

}
