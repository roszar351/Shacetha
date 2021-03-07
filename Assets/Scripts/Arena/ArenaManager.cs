using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

// Controls arena behaviour i.e. keeps track of current level, updates max level reached, responsible for spawning enemies and bosses
public class ArenaManager : MonoBehaviour
{
    // Using gameobject over transform provides more flexibility for future if wanted to add an spawn animation, effect, sprite or something similar
    // First index i.e. spawnPoints[0] is always the boss spawn point
    public GameObject[] spawnPoints;
    // Switched to using seperate arrays for different difficulties of enemies
    //public GameObject[] enemies;
    //public GameObject[] bosses;

    // Helps increase difficulty as player reaches higher levels
    public GameObject[] easyEnemies;
    public GameObject[] mediumEnemies;
    public GameObject[] hardEnemies;

    // Mostly used for variance but allows possibility for expanding boss pools in the future
    public GameObject[] easyBosses;
    public GameObject[] mediumBosses;
    public GameObject[] hardBosses;

    public GameObject[] arenas;
    public TextMeshProUGUI levelText;

    private int _level = 0;
    //private float _levelTimer = 0f; // might be used for score or some other stat
    // Keep track of enemies left and killed to start next level and when rewards should be given
    private int _enemiesLeft = 0;
    private int _enemiesKilled = 0;
    
    // Variables responsible for increasing difficulty as player reaches higher levels
    private int _easyEnemyCounter = 0;
    private int _mediumEnemyCounter = 0;
    private int _hardEnemyCounter = 0;
    private int _easyBossCounter = 0;
    private int _mediumBossCounter = 0;
    private int _hardBossCounter = 0;
    
    private bool _isRewardUIOpen = false;
    private bool _bossFight = false;
    private bool _nextLevelTransmission = false;
    private AudioManager _audioManager;

    [Tooltip("Controls how many enemies need to be defeated before player is eligible for a reward, bosses always trigger reward screen after being defeated.")]
    [SerializeField] private int rewardScreenRequirement = 5;
    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private so_GameEvent onRewardUIOpen;
    [SerializeField] private so_GameEvent onRewardUIClose;
    [SerializeField] private LevelLoader fadeScript;

    private void Start()
    {
        _audioManager = AudioManager.instance;
        levelText.SetText("Level: " + (_level+1));
        foreach (GameObject g in arenas)
        {
            g.SetActive(false);
        }
        arenas[0].SetActive(true);
    }

    private void Update()
    {
        if(!_nextLevelTransmission && _enemiesLeft <= 0 && !_isRewardUIOpen)
        {
            NextLevel();
        }
        /* For testing purposes only! */
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     Debug.LogError("Next level! Should only be used for testing!");
        //     NextLevel();
        // }
    }

    public void EnemyDied()
    {
        _enemiesLeft -= 1;
        _enemiesKilled++;

        // At least 1 enemy is still alive therefore dont check for reward screen
        if (_enemiesLeft > 0)
            return;
        
        // show reward screen after a boss fight or if enough enemies were defeated
        if(_bossFight)
        {
            _bossFight = false;
            onRewardUIOpen.Raise();
            AudioManager.instance.PlayRandomMusic(MusicType.GameMusic);
        }
        else if(_enemiesKilled / rewardScreenRequirement > 0)
        {
            onRewardUIOpen.Raise();
        }
    }

    public void RewardUIToggled(bool openOrClose)
    {
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
        _nextLevelTransmission = true;
        StartCoroutine(StartNextLevel());
    }

    private IEnumerator StartNextLevel()
    {
        _audioManager.PlayOneShotSound("NextLevel");
        _level++;
        levelText.SetText("Level: " + _level);
        // Switch to different arena
        switch (_level)
        {
            case 11:
                fadeScript.EndFadeAnimation();
                arenas[0].SetActive(false);
                arenas[1].SetActive(true);
                break;
            case 21:
                fadeScript.EndFadeAnimation();
                arenas[1].SetActive(false);
                arenas[2].SetActive(true);
                break;
            case 31:
                fadeScript.EndFadeAnimation();
                arenas[2].SetActive(false);
                arenas[3].SetActive(true);
                break;
        }
        
        yield return new WaitForSeconds(1f);

        if (_level % 5 == 0)
        {
            SpawnBosses();
        }
        else
        {
            SpawnEnemies();
        }

        _nextLevelTransmission = false;
    }

    private void SpawnEnemies()
    {
        // for each enemy to spawn, spawn enemy, enemiesLeft++
        //int howMany = 1 + (int)(0.4 * _level); // old formula for how many enemies should be spawned
        _easyEnemyCounter++;
        if (_easyEnemyCounter - 5 == 0)
        {
            _easyEnemyCounter = 0;
            _mediumEnemyCounter++;
        }
        if (_mediumEnemyCounter - 4 == 0)
        {
            _mediumEnemyCounter = 0;
            _hardEnemyCounter++;
        }

        EnemySpawner(_easyEnemyCounter, easyEnemies);
        EnemySpawner(_mediumEnemyCounter, mediumEnemies);
        EnemySpawner(_hardEnemyCounter, hardEnemies);
        
        /* Replaced these for loops with the EnemySpawner method to clean up the code
        int whichEnemy, whichPoint;
        Vector3 randomOffset;
        for (int i = 0; i < _easyEnemyCounter; ++i)
        {
            whichEnemy = Random.Range(0, easyEnemies.Length);
            whichPoint = Random.Range(0, spawnPoints.Length);

            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            //spawnPoints[whichPoint].SetActive(true);
            Instantiate(easyEnemies[whichEnemy], spawnPoints[whichPoint].transform.position + randomOffset, Quaternion.identity, enemiesParent.transform);
            _enemiesLeft++;
        }
        
        for (int i = 0; i < _mediumEnemyCounter; ++i)
        {
            whichEnemy = Random.Range(0, mediumEnemies.Length);
            whichPoint = Random.Range(0, spawnPoints.Length);

            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            //spawnPoints[whichPoint].SetActive(true);
            Instantiate(mediumEnemies[whichEnemy], spawnPoints[whichPoint].transform.position + randomOffset, Quaternion.identity, enemiesParent.transform);
            _enemiesLeft++;
        }
        
        for (int i = 0; i < _hardEnemyCounter; ++i)
        {
            whichEnemy = Random.Range(0, hardEnemies.Length);
            whichPoint = Random.Range(0, spawnPoints.Length);

            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            //spawnPoints[whichPoint].SetActive(true);
            Instantiate(hardEnemies[whichEnemy], spawnPoints[whichPoint].transform.position + randomOffset, Quaternion.identity, enemiesParent.transform);
            _enemiesLeft++;
        }
        */
    }

    private void EnemySpawner(int howMany, IReadOnlyList<GameObject> enemies)
    {
        int whichEnemy, whichPoint;
        Vector3 randomOffset;

        for (int i = 0; i < howMany; ++i)
        {
            whichEnemy = Random.Range(0, enemies.Count);
            whichPoint = Random.Range(0, spawnPoints.Length);

            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            //spawnPoints[whichPoint].SetActive(true); // can be used to play an animation attached to the spawn point, currently not used
            Instantiate(enemies[whichEnemy], spawnPoints[whichPoint].transform.position + randomOffset, Quaternion.identity, enemiesParent.transform);
            _enemiesLeft++;
        }
    }

    private void SpawnBosses()
    {
        _bossFight = true;
        
        _easyBossCounter++;
        if (_easyBossCounter - 4 == 0)
        {
            _easyBossCounter = 0;
            _mediumBossCounter++;
        }
        if (_mediumBossCounter - 3 == 0)
        {
            _mediumBossCounter = 0;
            _hardBossCounter++;
        }
        
        BossSpawner(_easyBossCounter, easyBosses);
        BossSpawner(_mediumBossCounter, mediumBosses);
        BossSpawner(_hardBossCounter, hardBosses);
        
        /* Replaced these for loops with the BossSpawner method to clean up the code
        Vector3 randomOffset;
        for (int i = 0; i < _easyBossCounter; ++i)
        {
            int whichBoss = Random.Range(0, easyBosses.Length);
            if (Regex.IsMatch(easyBosses[whichBoss].name, "^SlimeBoss.*$"))
            {
                //Debug.Log("Spawning slime!");
                _enemiesLeft += 7;
            }
            else
            {
                //Debug.Log("Spawning Ghost!");
                _enemiesLeft++;
            }
            
            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            Instantiate(easyBosses[whichBoss], spawnPoints[0].transform.position + randomOffset * 2, Quaternion.identity, enemiesParent.transform);
        }
        
        for (int i = 0; i < _mediumBossCounter; ++i)
        {
            int whichBoss = Random.Range(0, mediumBosses.Length);
            if (Regex.IsMatch(mediumBosses[whichBoss].name, "^SlimeBoss.*$"))
            {
                //Debug.Log("Spawning slime!");
                _enemiesLeft += 7;
            }
            else
            {
                //Debug.Log("Spawning Ghost!");
                _enemiesLeft++;
            }
            
            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            Instantiate(mediumBosses[whichBoss], spawnPoints[0].transform.position + randomOffset * 2, Quaternion.identity, enemiesParent.transform);
        }
        
        for (int i = 0; i < _hardBossCounter; ++i)
        {
            int whichBoss = Random.Range(0, hardBosses.Length);
            if (Regex.IsMatch(hardBosses[whichBoss].name, "^SlimeBoss.*$"))
            {
                //Debug.Log("Spawning slime!");
                _enemiesLeft += 15;
            }
            else
            {
                //Debug.Log("Spawning Ghost!");
                _enemiesLeft++;
            }
            
            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            Instantiate(hardBosses[whichBoss], spawnPoints[0].transform.position + randomOffset * 2, Quaternion.identity, enemiesParent.transform);
        }
        */

        _audioManager.PlayRandomMusic(MusicType.BossMusic);
    }

    private void BossSpawner(int howMany, IReadOnlyList<GameObject> bosses)
    {
        int whichBoss;
        Vector3 randomOffset;

        for (int i = 0; i < howMany; ++i)
        {
            whichBoss = Random.Range(0, bosses.Count);
            if (Regex.IsMatch(bosses[whichBoss].name, "^SlimeBoss.*$"))
            {
                //Debug.Log("Spawning slime!");
                _enemiesLeft += 7;
            }
            else
            {
                //Debug.Log("Spawning Ghost!");
                _enemiesLeft++;
            }
            
            randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            Instantiate(bosses[whichBoss], spawnPoints[0].transform.position + randomOffset * 2, Quaternion.identity, enemiesParent.transform);
        }
    }
}
