using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mostly used to find audio manager which doesnt get destroyed between scenes to use in onclick events
public class ReferenceKeeper : MonoBehaviour
{
    #region
    public static ReferenceKeeper instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Tried to create another instance of " + this.name);
        }
    }
    #endregion

    [HideInInspector]
    public AudioManager audioManager;
    [HideInInspector]
    public LevelLoader levelLoader;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void PlaySound(string s)
    {
        if (audioManager == null)
            return;
        audioManager.PlaySound(s);
    }

    public void PlayButtonClicked()
    {
        if (audioManager == null)
            return;
        audioManager.PlaySound("ButtonClicked");
    }

    public void PlayMouseOver()
    {
        if (audioManager == null)
            return;
        audioManager.PlaySound("MouseOver");
    }

    public void UpdateMasterVolume(float masterVolume)
    {
        audioManager.UpdateMasterVolume(masterVolume);
    }

    public void UpdateMusicVolume(float musicVolume)
    {
        audioManager.UpdateMusicVolume(musicVolume);
    }

    public void UpdateEffectsVolume(float effectsVolume)
    {
        audioManager.UpdateEffectsVolume(effectsVolume);
    }
}
