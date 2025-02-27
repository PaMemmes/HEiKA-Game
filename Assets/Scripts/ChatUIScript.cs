using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }


    [SerializeField] private Transform ChatViewContent;
    [SerializeField] private GameObject Chat_Line_Pref;
    [SerializeField] private Scrollbar scrollbar;

    public void addText(string text)
    {
        GameObject textLine = Instantiate(Chat_Line_Pref, ChatViewContent);
        textLine.GetComponent<Text>().text = text;
    }
}
