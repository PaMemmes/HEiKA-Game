using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror; // Required for network communication

public class AutoPilotScript : NetworkBehaviour
{
    public TMP_Dropdown themenDropdown;
    public Transform fragenScrollViewContent;

    [SyncVar] private bool isNextQuestionTriggered = false; // Triggered from Patient
    private bool isAutoPilotActive = false;
    private int currentQuestionIndex = 0;
    private List<Thema.Frage> currentFragen;
    private ChatBehaviour artzChatBehaviour;

    void Start()
    {
        GameObject artz = GameObject.FindWithTag("Artz");
        if (artz != null)
        {
            artzChatBehaviour = artz.GetComponent<ChatBehaviour>();
            if (artzChatBehaviour == null)
            {
                Debug.LogError("ChatBehaviour not found on Artz!");
            }
        }
        else
        {
            Debug.LogError("Artz object not found!");
        }
    }

    [Server]
    public void StartAutoPilot()
    {
        if (artzChatBehaviour == null)
        {
            Debug.LogError("Cannot start AutoPilot: Artz's ChatBehaviour is missing.");
            return;
        }

        isAutoPilotActive = true;
        currentQuestionIndex = 0;

        int selectedThemaIndex = themenDropdown.value;
        currentFragen = GetFragenFromScrollView(selectedThemaIndex);

        if (currentFragen.Count > 0)
        {
            Debug.Log("Starting co routine");
            StartCoroutine(AskQuestionsWhenButtonClicked());
        }
        else
        {
            Debug.LogWarning("No questions found in the selected theme.");
        }
    }

    public void StopAutoPilot()
    {
        isAutoPilotActive = false;
        StopAllCoroutines();
    }

    private List<Thema.Frage> GetFragenFromScrollView(int themaIndex)
    {
        List<Thema.Frage> fragen = new List<Thema.Frage>();
        Transform viewport = fragenScrollViewContent.Find("Viewport");
        if (viewport != null)
        {
            Transform content = viewport.Find("Content");
            if (content != null)
            {
                foreach (Transform child in content)
                {
                    FragenButtonScript fragenButtonScript = child.GetComponent<FragenButtonScript>();
                    if (fragenButtonScript != null)
                    {
                        fragen.Add(fragenButtonScript.frage);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Content object not found in the scroll view.");
            }
        }
        else
        {
            Debug.LogWarning("Viewport object not found in the scroll view.");
        }

        return fragen;
    }

    
    IEnumerator AskQuestionsWhenButtonClicked()
    {
        Debug.Log("question index:" + currentQuestionIndex);
        Debug.Log("question count:" + currentFragen.Count);
        Debug.Log("AutoPilot active:" + isAutoPilotActive);
        while (isAutoPilotActive && currentQuestionIndex < currentFragen.Count)
        {
            if (currentQuestionIndex > 0)
            {
                isNextQuestionTriggered = false;
                Debug.Log("waiting for next question");
                yield return new WaitUntil(() => isNextQuestionTriggered);
            }

            if (artzChatBehaviour != null)
            {
                Debug.Log("Starting autopilot");
                artzChatBehaviour.AskQuestion(
                    currentFragen[currentQuestionIndex].Text.Trim(),
                    currentFragen[currentQuestionIndex].getAntworten()
                );
            }
            else
            {
                Debug.LogError("ChatBehaviour for Artz is missing!");
            }
            Debug.Log("Let's go to next question");
            currentQuestionIndex++;
        }

        // Reset state
        currentQuestionIndex = 0;
        isAutoPilotActive = false;
    }

    [Server]
    public void TriggerNextQuestion()
    {
        Debug.Log("Triggering next question from the server.");
        RpcTriggerNextQuestion(); // Inform all clients
    }

    [ClientRpc]
    void RpcTriggerNextQuestion()
    {
        Debug.Log("Received next question trigger on client.");
        isNextQuestionTriggered = true; // Set flag on clients
    }
}
