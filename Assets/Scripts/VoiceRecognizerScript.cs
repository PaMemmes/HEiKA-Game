using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows.Speech;

//Script zur Spracherkennung. Nutzt die Windows api, Spracherkennung muss in Windows aktivert sein.
public class VoiceRecognizerScript : MonoBehaviour
{/*
    [SerializeField] ChatBehaviour Chat;
    private DictationRecognizer dictationRecognizer;


    void Start()        //Startet die Spracherkennung
    {

        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        Debug.Log("Started voice recognition");
    }


    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)   //Wenn ein Satz verstanden wurde, wird dieser in den Chat gesendet
    {
        Debug.Log(text + " With Confidence: " + confidence);
        Chat.AutoSend(text);
    }


    public void activate(bool active)       //Aktiviert und deaktivier die Spracherkennung
    {
        if (active)
        {
            dictationRecognizer.Start();
            Debug.Log("Restarted voice recognition");
        }
        else
        {
            dictationRecognizer.Stop();
            Debug.Log("Paused voice recognition");
        }
    }

   
    void OnDestroy()        //Beendet die Spracherkennung
    {
        dictationRecognizer.Dispose();
        Debug.Log("Ended voice recognition");
    }*/
}
