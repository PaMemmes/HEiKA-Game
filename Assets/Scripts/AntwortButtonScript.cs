using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntwortButtonScript : MonoBehaviour
{
    [SerializeField] public Text buttonText;
    public string Antwort;

    public void setAntwort(string antwort) //ref?
    {
        Antwort = antwort;
        buttonText.text = Antwort.Trim();
    }
}