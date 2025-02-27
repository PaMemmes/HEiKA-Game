using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

//Script für das Optionsmenu. Bietet funktionen für die verschiedenen Einstellungen
public class OptionsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;       //Verknüpfung zum AudioMixer, dieser Verwaltet alle Sounds
    public Slider volumeSlider;         //Slider für die Lautstärke

    public Dropdown resolutionDropdown; //Dropdown für die verschiedenen Auflösungen
    private Resolution[] resolutions;   //Liste der Auflösungen

    // Awake is called before Start, Läd die verschiedenen Anfangswerte
    void Awake()
    {

        float currentValue;                                     //VolumeSlider wird auf ausgansposition gebracht
        audioMixer.GetFloat("Background", out currentValue);
        volumeSlider.value = currentValue;

        resolutions = Screen.resolutions;                       //mögliche Resolutions werden geladen
        resolutionDropdown.ClearOptions();                      //resolutionDropdown wird geleert...

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)            //...und wieder mit den aktuellen Möglichkeiten gefüllt
        {

            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz"; //So wird die Auflösung angezeigt
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

   
    public void SetVolumeBackground(float volume)   //Hintergrundlautstärke wird angepasst
    {

        //audioMixer.SetFloat("Background", volume);
    }

    public void SetVolumeFX(float volume)           //Effektlautstärke wird angepasst
    {

        audioMixer.SetFloat("FX", volume);
    }

    public void SetVolumeSpeech(float volume)       //Sprachlautstärke wird angepasst (aktuell nicht verfügbar)
    {

        audioMixer.SetFloat("Speech", volume);
    }

    public void SetFullscreen(bool isFullscreen)    //Vollbildmodus wird aktiviert/deaktiviert
    {

        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)  //Auflösung wird angepasst
    {

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
