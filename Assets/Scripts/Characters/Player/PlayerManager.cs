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
    public GameObject deathOverlay;

    [SerializeField]
    private so_NPCStats defaultPlayerStats;

    [SerializeField]
    private so_GameEvent onPlayerDied;

    private PlayerController _playerController;
    private bool _isPlayerAlive = true;

    private void Start()
    {
        _isPlayerAlive = true;
        ResetPlayerStats();
        deathOverlay.SetActive(false);
        _playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(!_isPlayerAlive && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopPlayerInput()
    {
        playerHands.gameObject.SetActive(false);
        _playerController.StopInput();
    }
    
    public void ResumePlayerInput()
    {
        playerHands.gameObject.SetActive(true);
        _playerController.ResumeInput();
    }

    public void SlowPlayMovement(float modifier)
    {
        _playerController.SlowMovement(modifier);
    }

    public void ResetPlayerMovementValue()
    {
        _playerController.ResetMovementValue();
    }

    public void DealDamageToPlayer(int damage)
    {
        _playerController.TakeDamage(damage);
    }

    public void KillPlayer()
    {
        _isPlayerAlive = false;
        _playerController.StopInput();
        onPlayerDied.Raise();
        AudioManager.instance.PlayOneShotSound("DeathScreen");

        Invoke(nameof(TurnOnDeathOverlay), .7f);
        Destroy(player, 1f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetPlayerStats()
    {
        player.GetComponent<PlayerController>().myStats.ResetStats(defaultPlayerStats);
    }

    private void TurnOnDeathOverlay()
    {
        deathOverlay.SetActive(true);
    }
}
