using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        AudioManager.instance.ResumeMusic();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            PlayerManager.instance.ResumePlayerInput();
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadMenu()
    {
        AudioManager.instance.PlayPauseMenuMusic();

        Time.timeScale = 1f;
        //SceneManager.LoadScene(0);
        ReferenceKeeper.instance.levelLoader.GoBackToMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Pause()
    {
        AudioManager.instance.PauseMusic();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            PlayerManager.instance.StopPlayerInput();
        }

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
