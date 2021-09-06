using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        //Lataa valitun scene, väliaikaisesti demo scenen
        //Pitäiskö lisätä joku loading screen tai vastaava btw
        SceneManager.LoadScene("DemoScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
