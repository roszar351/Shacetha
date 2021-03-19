using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highestArenaText;

    private ReferenceKeeper _referenceKeeper;

    private void Start()
    {
        _referenceKeeper = ReferenceKeeper.instance;
        if (PlayerPrefs.HasKey("MaxLevel"))
        {
            highestArenaText.SetText("Highest Level: " + PlayerPrefs.GetInt("MaxLevel"));
        }
        else
        {
            highestArenaText.SetText("Highest Level: " + 0);
        }
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

    public void GoToArena()
    {
        //SceneManager.LoadScene(2);
        _referenceKeeper.levelLoader.LoadSpecificLevel(2, false);
    }
}
