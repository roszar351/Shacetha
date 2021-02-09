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
        Destroy(player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetPlayerStats()
    {
        player.GetComponent<PlayerController>().myStats.ResetStats(defaultPlayerStats);
    }
}
