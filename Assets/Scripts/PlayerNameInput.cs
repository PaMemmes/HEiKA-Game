using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script mit dem der Spieler am Anfang seinen Namen eingibt
public class PlayerNameInput : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;  //Feld in dem der Spieler seinen Namen eingibt
    [SerializeField] private Button continueButton = null;          //Feld zum bestätigen und beitreten eines Servers

    public static string DisplayName { get; private set; }          //Spielername
    private const string PlayerPrefsNameKey = "Playername";

    // Start is called before the first frame update
    private void Start() => SetUpInputField();

    private void SetUpInputField()      //Am Programstart aufgerufen, Defaultname/letzter benutzter Name wird im eingabefeld vorbereitet
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)  //Spielername wird aktualiert
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);   //Button zum serverbeitritt wir blockirt, fals der Name leer ist
    }

    public void SavePlayerName()            //Spielername wird für nächstes Mal gespeichert
    {
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
