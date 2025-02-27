using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FragenButtonScript : MonoBehaviour
{
    [SerializeField] public Text buttonText;
    public Thema.Frage frage;

    public void setFrage(Thema.Frage newFrage) //ref?
    {
        frage = new Thema.Frage();
        frage = newFrage;
        buttonText.text = frage.Text.Trim();
    }
}
