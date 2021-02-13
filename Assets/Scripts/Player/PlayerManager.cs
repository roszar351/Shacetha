using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;
    public HandsController playerHands;
    public Inventory playerInventory;

    [SerializeField]
    private so_NPCStats defaultPlayerStats;

    [SerializeField]
    private so_GameEvent onPlayerDied;

    private PlayerController playerController;

    private void Start()
    {
        ResetPlayerStats();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopPlayerInput()
    {
        playerHands.gameObject.SetActive(false);
        playerController.StopInput();
    }
    
    public void ResumePlayerInput()
    {
        playerHands.gameObject.SetActive(true);
        playerController.ResumeInput();
    }

    public void KillPlayer()
    {
        onPlayerDied.Raise();
        AudioManager.instance.PlayOneShotSound("DeathScreen"); //TODO: Implement a death screen currently only plays a sound
        Destroy(player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetPlayerStats()
    {
        player.GetComponent<PlayerController>().myStats.ResetStats(defaultPlayerStats);
    }
}
