using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;

    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrResume();
        }

    }

    public void PauseOrResume()
    {
        if (Paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        Paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void Resume()
    {
        Paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Return()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
