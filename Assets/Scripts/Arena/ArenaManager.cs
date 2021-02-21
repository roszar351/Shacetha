using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaManager : MonoBehaviour
{
    // Using gameobject over transform provides more flexibility for future if wanted to add an spawn animation, effect, sprite or something similar
    // First index i.e. spawnPoints[0] is always the boss spawn point
    public GameObject[] spawnPoints;
    // Keep track of traps present in the arena to be activated, currently not sure if it will be used
    public GameObject[] arenaTraps;
    // Currently using one array for enemies, but might need to split them to have more dangerous/rare enemies be part of a different array
    public GameObject[] enemies;
    public GameObject[] bosses;

    public TextMeshProUGUI levelText;

    private int _level = 0;
    private float _levelTimer = 0f; // might be used for score or some other stat
    private int _enemiesLeft = 0;
    private int _enemiesKilled = 0;
    private bool _isRewardUIOpen = false;
    private bool _bossFight = false;
    private AudioManager _audioManager;

    [SerializeField]
    private int rewardScreenRequirement = 5;

    [SerializeField]
    private GameObject enemiesParent;

    [SerializeField]
    private so_GameEvent onRewardUIOpen;
    
    [SerializeField]
    private so_GameEvent onRewardUIClose;

    private void Start()
    {
        _audioManager = AudioManager.instance;
        levelText.SetText("Level: " + (_level+1));
    }

    private void Update()
    {
        if(_enemiesLeft <= 0 && !_isRewardUIOpen)
        {
            NextLevel();
        }
    }

    public void EnemyDied()
    {
        _enemiesLeft -= 1;
        _enemiesKilled++;

        if(_enemiesLeft <= 0 && _bossFight)
        {
            _bossFight = false;
            onRewardUIOpen.Raise();
            AudioManager.instance.PlayRandomMusic(MusicType.GameMusic);
        }
        else if(_enemiesLeft <= 0 && _enemiesKilled / rewardScreenRequirement > 0)
        {
            onRewardUIOpen.Raise();
        }
    }

    public void RewardUIToggled(bool openOrClose)
    {
        // TODO: currently reward UI is based on enemies killed and after each boss
        _isRewardUIOpen = openOrClose;
        _enemiesKilled = 0;
    }

    public void UpdateMaxLevel()
    {
        if (PlayerPrefs.HasKey("MaxLevel"))
        {
            if (_level > PlayerPrefs.GetInt("MaxLevel"))
            {
                PlayerPrefs.SetInt("MaxLevel", _level);
            }
        }
        else
        {
            PlayerPrefs.SetInt("MaxLevel", _level);
        }

        PlayerPrefs.Save();
    }

    private void NextLevel()
    {
        _audioManager.PlayOneShotSound("NextLevel");
        _level++;
        levelText.SetText("Level: " + _level);
        if (_level % 5 == 0)
        {
            SpawnBoss();
        }
        else
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        //TODO: spawn enemies, rework spawn system
        // for each enemy to spawn, spawn enemy, enemiesLeft++
        int howMany = 2 + (int)(1.5 * _level);
        int whichEnemy, whichPoint;
        Vector3 randomOffset;

        for (int i = 0; i < howMany; ++i)
        {
            whichEnemy = Random.Range(0, enemies.Length);
            whichPoint = Random.Range(0, spawnPoints.Length);

            randomOffset = new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f), 0f);

            //spawnPoints[whichPoint].SetActive(true);
            Instantiate(enemies[whichEnemy], spawnPoints[whichPoint].transform.position + randomOffset, Quaternion.identity, enemiesParent.transform);
            _enemiesLeft++;
        }
    }

    private void SpawnBoss()
    {
        _bossFight = true;

        int howMany = _level / 5;
        Vector3 randomOffset;

        for (int i = 0; i < howMany; ++i)
        {
            int whichBoss = Random.Range(0, bosses.Length);
            if (bosses[whichBoss].name == "SlimeBoss")
            {
                Debug.Log("Spawning slime!");
                _enemiesLeft += 7;
            }
            else
            {
                Debug.Log("Spawning Ghost!");
                _enemiesLeft++;
            }
            
            randomOffset = new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-0.7f, 0.7f), 0f);

            Instantiate(bosses[whichBoss], spawnPoints[0].transform.position + randomOffset * 2, Quaternion.identity, enemiesParent.transform);
        }

        _audioManager.PlayRandomMusic(MusicType.BossMusic);
    }

    private void ActivateTraps()
    {
        //TODO: activate traps, maybe add a method to disable as well or just leave activated for rest of the game
    }
}
