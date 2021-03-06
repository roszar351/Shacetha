using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    
    private static readonly int StartFade = Animator.StringToHash("StartFade");
    private static readonly int EndFadeTrigger = Animator.StringToHash("EndFadeTrigger");

    private void Start()
    {
        EndFadeAnimation();
    }

    public void LoadNextLevelWithStory()
    {
        AudioManager.instance.PlayRandomMusic(MusicType.GameMusic);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, true));
    }

    public void LoadNextLevelWithNoStory()
    {
        AudioManager.instance.PlayRandomMusic(MusicType.GameMusic);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, false));
    }

    public void LoadSpecificLevel(int buildIndex, bool withStory)
    {
        AudioManager.instance.PlayRandomMusic(MusicType.GameMusic);
        StartCoroutine(LoadLevel(buildIndex, withStory));
    }

    public void GoBackToMenu()
    {
        AudioManager.instance.PlayRandomMusic(MusicType.MenuMusic);
        StartCoroutine(LoadLevel(0, false));
    }

    public void StartFadeAnimation()
    {
        transition.SetTrigger(StartFade);
    }

    public void EndFadeAnimation()
    {
        //Debug.Log("Playing fade animation");
        transition.SetTrigger(EndFadeTrigger);
    }

    IEnumerator LoadLevel(int levelIndex, bool withStory)
    {
        // Disable player input while level is loading
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            PlayerManager.instance.StopPlayerInput();
        }

        if (withStory)
        {
            // Can be used to add a cutscene, story text or something else while the level is being loaded.
        }
        else
        {
            transition.SetTrigger(StartFade);
            yield return new WaitForSeconds(.9f);
        }

        SceneManager.LoadScene(levelIndex);
    }
}
