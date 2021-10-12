using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] GameObject settingsMenu;

    bool Settings = false;

    public void ToggleWindowMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void SetVolume(float sliderValue)
    {
        mainMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    public void ToggleSettingsPanel()
    {
        if (Settings)
        {
            settingsMenu.SetActive(false);
            Settings = false;
        }
        else
        {
            settingsMenu.SetActive(true);
            Settings = true;
        }
    }
}
