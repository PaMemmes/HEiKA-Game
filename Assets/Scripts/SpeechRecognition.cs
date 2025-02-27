using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;



public class SpeechRecognition : MonoBehaviour
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    [DllImport("__Internal")]
    private static extern void SpeakText(string str);

    [DllImport("__Internal")]
    private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();

    [DllImport("__Internal")]
    private static extern void BindWebGLTexture(int texture);
#endif

    public void Speak(string str)
    {
        Debug.Log("Speaking: " + str);
        #if UNITY_WEBGL
            SpeakText(str);
        #else
            Debug.LogWarning("Keine Sprachausgabe für diese Platform");
        #endif
        Debug.Log("I have Spoken");
    }

}
