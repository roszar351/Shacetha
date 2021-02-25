using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public TextMeshProUGUI highestArenaText;

    private ReferenceKeeper _referenceKeeper;

    private void Start()
    {
        _referenceKeeper = ReferenceKeeper.instance;
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene(1);
        _referenceKeeper.levelLoader.LoadNextLevelWithNoStory();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenOptionMenu()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToArena()
    {
        //SceneManager.LoadScene(2);
        _referenceKeeper.levelLoader.LoadSpecificLevel(2, false);
    }
}
