using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool Settings = false;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    public void StartGame()
    {

        //TODO: 
        //Async loading screen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
        } else
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            Settings = true;
        }
    }
}
