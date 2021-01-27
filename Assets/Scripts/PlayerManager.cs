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

    private void Start()
    {
        ResetPlayerStats();
    }

    public void KillPlayer()
    {
        Destroy(player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetPlayerStats()
    {
        player.GetComponent<PlayerController>().myStats.ResetStats(defaultPlayerStats);
    }
}
