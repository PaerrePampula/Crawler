using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    bool Settings = false;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject runLengthPopup;
    [SerializeField] GenerationSettings longSettings;
    [SerializeField] GenerationSettings shortSettings;
    [SerializeField] AudioClip buttonPress;
    [SerializeField] AudioMixer mainMixer;

    public void StartGame()
    {

        //TODO: 
        //Async loading screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void StartGameWithSettings(string length)
    {
        if (length == "long")
        {
            Globals.GenerationSettings = longSettings;
        }
        else
        {
            Globals.GenerationSettings = shortSettings;
        }
    }
    public void EnablePopupForRunSettings()
    {
        runLengthPopup.gameObject.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeMenu()
    {
        if (Settings)
        {
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);

            Settings = false;
        } 
        else
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            runLengthPopup.SetActive(false);
            Settings = true;
        }
    }

    public void ToggleWindowMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void SetVolume(float sliderValue)
    {
        mainMixer.SetFloat("MasterVol", Mathf.Log10 (sliderValue) * 20);
    }
}
[System.Serializable]
class GenerationSettings
{
    [SerializeField] int maxDistanceToBoss;
    [SerializeField] int minDistanceToBoss;
    [SerializeField] int maxMapWidth;
    [SerializeField] int maxMapHeight;
    [SerializeField] int maxBranchingPathLength;
    [SerializeField] int chanceForBranchingPathsToDiverge;

    public int MaxDistanceToBoss { get => maxDistanceToBoss; set => maxDistanceToBoss = value; }
    public int MinDistanceToBoss { get => minDistanceToBoss; set => minDistanceToBoss = value; }
    public int MaxMapWidth { get => maxMapWidth; set => maxMapWidth = value; }
    public int MaxMapHeight { get => maxMapHeight; set => maxMapHeight = value; }
    public int MaxBranchingPathLength { get => maxBranchingPathLength; set => maxBranchingPathLength = value; }
    public int ChanceForBranchingPathsToDiverge { get => chanceForBranchingPathsToDiverge; set => chanceForBranchingPathsToDiverge = value; }
}