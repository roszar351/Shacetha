using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    [SerializeField] private GameObject characterSheet;
    [SerializeField] private GameObject inventory;
    private bool _charSheetAssigned = true;
    private bool _inventoryAssigned = true;

    private void Start()
    {
        _charSheetAssigned = characterSheet != null;
        _inventoryAssigned = inventory != null;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool pause = true;
            if (_charSheetAssigned)
            {
                if (characterSheet.activeSelf)
                {
                    characterSheet.SetActive(false);
                    pause = false;
                }
            }

            if (_inventoryAssigned)
            {
                if (inventory.activeSelf)
                {
                    inventory.SetActive(false);
                    pause = false;
                }
            }

            if (pause)
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
    }

    public void Resume()
    {
        AudioManager.instance.ResumeMusic();
        AudioManager.instance.StopPauseMenuMusic();

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
        AudioManager.instance.StopMusic();
        AudioManager.instance.StopPauseMenuMusic();
        AudioManager.instance.PlayRandomMusic(MusicType.MenuMusic);

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
        AudioManager.instance.PlayPauseMenuMusic();

        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            PlayerManager.instance.StopPlayerInput();
        }

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
