using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardUIParent;
    public GameObject[] choiceButtons;
    //TODO: will have to change how this item list will work as currently all items are dragged in through editor and picked randomly
    // current idea is to have another scriptable object called item pool e.g. normal, rare, very rare and have rewward manager pick item from one of the pools depending
    // on a random number roll
    public List<so_Item> allItems;

    private List<so_Item> _itemPool;
    private InventorySlot[] _icons;
    private TextMeshProUGUI[] _descriptions;

    [SerializeField]
    private so_NPCStats playerStats;
    [SerializeField]
    private so_Item nullItem;

    private so_Item _item1;
    private so_Item _item2;

    // first index indicates which stat is to be improved
    // second index indicates by how much
    private int[] _statReward1;
    private int[] _statReward2;
    // first and third index indicates which stat is to be improved
    // second and fourth index indicates by how much
    private int[] _statReward3;
    private int _healReward = 0;

    private int _lowStatStartRange = 1;
    private int _averageStatStartRange = 3;
    private int _highStatStartRange = 5;
    private int _veryHighStatStartRange = 8;
    private int _extremeStatStartRange = 11;

    private string[] _statStrings = new string[] { " max HP", " attack damage", " base armor" };

    [SerializeField]
    private so_GameEvent onRewardUIOpen;
        
    [SerializeField]
    private so_GameEvent onRewardUIClose;

    private void Start()
    {
        _icons = new InventorySlot[choiceButtons.Length];
        _descriptions = new TextMeshProUGUI[choiceButtons.Length];

        for (int i = 0; i < 4; ++i)
        {
            _icons[i] = choiceButtons[i].GetComponentInChildren<InventorySlot>();
            foreach (TextMeshProUGUI t in choiceButtons[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (!t.gameObject.name.Equals("Description")) continue;
                _descriptions[i] = t;
                break;
            }
        }

        _itemPool = new List<so_Item>();
        _itemPool.AddRange(allItems);

        _statReward1 = new int[2];
        _statReward2 = new int[2];
        _statReward3 = new int[4];

        DisableButtons();
    }

    private void Update()
    {
        /* for testing
        if(Input.GetKeyDown(KeyCode.R))
        {
            OpenRewardScreen();
        }
        */
    }

    public void OpenRewardScreen()
    {
        if (rewardUIParent.activeSelf)
        {
            return;
        }
        
        StartCoroutine(nameof(RewardSetup));
    }

    IEnumerator RewardSetup()
    {
        RollNewItemRewards();
        RollStatReward();
        RollHealReward();
        EnableButtons();

        yield return new WaitForSeconds(1f);
        
        AudioManager.instance.PlayOneShotSound("RewardUI");
        rewardUIParent.SetActive(true);
    }

    public void PickReward(int whichReward)
    {
        int healamount = 5;
        DisableButtons();
        switch (whichReward)
        {
            case 0:
            {
                if (_item1 != null && _item1.itemType != ItemType.NULL)
                {
                    PlayerManager.instance.playerInventory.Add(_item1);
                    _itemPool.Remove(_item1);
                }

                ApplyStatReward(0);
                break;
            }
            case 1:
            {
                if (_item1 != null && _item1.itemType != ItemType.NULL)
                {
                    PlayerManager.instance.playerInventory.Add(_item2);
                    _itemPool.Remove(_item2);
                }

                ApplyStatReward(1);
                break;
            }
            case 2:
                ApplyStatReward(2);
                break;
            case 3:
                healamount = _healReward;
                break;
            default:
                Debug.LogError("Picked reward with index out of bounds!");
                return;
        }

        _item1 = nullItem;
        _item2 = nullItem;
        _icons[0].AddItem(_item1);
        _icons[1].AddItem(_item2);
        
        rewardUIParent.SetActive(false);
        onRewardUIClose.Raise();

        //Debug.Log("Healing after picking reward!");
        PlayerManager.instance.player.GetComponent<PlayerController>().Heal(healamount);
    }

    private void RollNewItemRewards()
    {
        int firstRanNum = 0;
        // If no items left assign dummy items to rewards
        // If 1 item left assign that to reward 1 and then assign dummy item to reward 2
        // Else just assign random items to the rewards
        if (_itemPool.Count < 1)
        {
            _item1 = nullItem;
            _item2 = nullItem;

            choiceButtons[0].GetComponent<Button>().interactable = false;
            choiceButtons[1].GetComponent<Button>().interactable = false;
        }
        else
        {
            do
            {
                firstRanNum = Random.Range(0, _itemPool.Count);
            } while (_itemPool[firstRanNum] == null);

            _item1 = _itemPool[firstRanNum];
            //allItems[ranNum] = null;

            if (_itemPool.Count < 2)
            {
                _item2 = nullItem;
                choiceButtons[1].GetComponent<Button>().interactable = false;
            }
            else
            {
                int secondRanNum = 0;
                do
                {
                    secondRanNum = Random.Range(0, _itemPool.Count);
                } while (_itemPool[secondRanNum] == null || firstRanNum == secondRanNum);

                _item2 = _itemPool[secondRanNum];
                //allItems[ranNum] = null;
            }
        }

        _icons[0].AddItem(_item1);
        _icons[1].AddItem(_item2);
    }

    private void RollStatReward()
    {
        // assume no extra stat award was gained
        _statReward1[0] = -1;
        _statReward2[0] = -1;
        _statReward3[2] = -1;

        int randomNum = 0;
        int statValue = 0;
        int multiplier = 0;
        int actualStatValue = 0;

        // 0 = maxhp
        // 1 = damage
        // 2 = armor
        int whichStat = 0;

        // odds for additional stat reward with items
        // 5% for negative stat reward
        // 10% for positive stat reward
        // 85% for no stat reward

        // Additional stat with items
        // Currently only 2 item rewards planned.
        for (int i = 0; i < 2; ++i)
        {
            switch (i)
            {
                case 0 when _item1 == nullItem:
                case 1 when _item2 == nullItem:
                    continue;
            }

            randomNum = Random.Range(0, 100);
            if (randomNum < 5)
            {
                multiplier = -1;
            }
            else if (randomNum < 15)
            {
                multiplier = 1;
            }
            else
            {
                multiplier = 0;
            }

            // odds for level of stat reward
            // low       - 15%      1-2
            // average   - 70%      3-4
            // high      - 10%      5-7
            // very high - 4%       8-10
            // extreme   - 1%       11-12

            if (multiplier != 0)
            {
                statValue = GetStatValue();
                actualStatValue = statValue * multiplier;
                whichStat = Random.Range(0, 3);

                _descriptions[i].SetText("and\n" + actualStatValue + _statStrings[whichStat]);

                switch (i)
                {
                    case 0:
                        _statReward1[0] = whichStat;
                        _statReward1[1] = actualStatValue;
                        break;
                    case 1:
                        _statReward2[0] = whichStat;
                        _statReward2[1] = actualStatValue;
                        break;
                    default:
                        Debug.LogError("Error with assigning stat rewards to items.");
                        break;
                }
            }
            else
            {
                _descriptions[i].SetText("");
            }
        }

        // odds for stat reward i.e. third reward
        // 80% positive
        // 20% negative
        // odds for additional stat reward with the stat reward
        // 10% for additional stat reward
        // 30% for additional stat reward to be negative
        // 70% for additional stat reward to be positive

        randomNum = Random.Range(0, 100);
        // positive or negative stat reward
        if (randomNum < 20)
        {
            multiplier = -1;
        }
        else
        {
            multiplier = 1;
        }

        statValue = GetStatValue();

        actualStatValue = statValue * multiplier;
        whichStat = Random.Range(0, 3);

        string tempStr = actualStatValue + _statStrings[whichStat];
        _statReward3[0] = whichStat;
        _statReward3[1] = actualStatValue;

        // additional stat reward
        randomNum = Random.Range(0, 100);
        if (randomNum < 10)
        {
            randomNum = Random.Range(0, 100);
            if (randomNum < 30)
            {
                multiplier = -1;
            }
            else
            {
                multiplier = 1;
            }
        }
        else
        {
            multiplier = 0;
        }

        if (multiplier != 0)
        {
            statValue = GetStatValue();
            actualStatValue = statValue * multiplier;
            do
            {
                whichStat = Random.Range(0, 3);
            } while (_statReward3[0] == whichStat);

            tempStr += "\nand\n" + actualStatValue + _statStrings[whichStat];
            _statReward3[2] = whichStat;
            _statReward3[3] = actualStatValue;
        }

        _descriptions[2].SetText(tempStr);
    }

    private void RollHealReward()
    {
        // currently heal player for 25% of their max hp
        _healReward = playerStats.maxHp / 4;
        _descriptions[3].SetText("Heal for " + _healReward);

    }

    private void ApplyStatReward(int whichReward)
    {
        if(whichReward == 0)
        {
            switch(_statReward1[0])
            {
                case 0:
                    playerStats.UpdateMaxHp(_statReward1[1]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(_statReward1[1]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(_statReward1[1]);
                    break;
                default:
                    break;
            }
        }
        else if(whichReward == 1)
        {
            switch (_statReward2[0])
            {
                case 0:
                    playerStats.UpdateMaxHp(_statReward2[1]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(_statReward2[1]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(_statReward2[1]);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (_statReward3[0])
            {
                case 0:
                    playerStats.UpdateMaxHp(_statReward3[1]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(_statReward3[1]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(_statReward3[1]);
                    break;
                default:
                    break;
            }

            switch (_statReward3[2])
            {
                case 0:
                    playerStats.UpdateMaxHp(_statReward3[3]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(_statReward3[3]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(_statReward3[3]);
                    break;
                default:
                    break;
            }
        }
    }

    private int GetStatValue()
    {
        int randomNum = Random.Range(0, 100);
        int statValue = 0;

        if (randomNum < 15)
        {
            statValue = Random.Range(_lowStatStartRange, _averageStatStartRange);
        }
        else if (randomNum < 85)
        {
            statValue = Random.Range(_averageStatStartRange, _highStatStartRange);
        }
        else if (randomNum < 95)
        {
            statValue = Random.Range(_highStatStartRange, _veryHighStatStartRange);
        }
        else if (randomNum < 99)
        {
            statValue = Random.Range(_veryHighStatStartRange, _extremeStatStartRange);
        }
        else
        {
            statValue = Random.Range(_extremeStatStartRange, _extremeStatStartRange + 2);
        }

        return statValue;
    }

    private void EnableButtons()
    {
        foreach(GameObject g in choiceButtons)
        {
            g.GetComponent<Button>().interactable = true;
        }
    }

    private void DisableButtons()
    {
        foreach (GameObject g in choiceButtons)
        {
            g.GetComponent<Button>().interactable = false;
        }
    }
}
