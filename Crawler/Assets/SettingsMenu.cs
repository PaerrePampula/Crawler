using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;

    public void ToggleWindowMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void SetVolume(float sliderValue)
    {
        mainMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }
}
