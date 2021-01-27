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

    private List<so_Item> itemPool;
    private InventorySlot[] icons;
    private TextMeshProUGUI[] descriptions;

    [SerializeField]
    private so_NPCStats playerStats;
    [SerializeField]
    private so_Item nullItem;

    private so_Item item1;
    private so_Item item2;

    // first index indicates which stat is to be improved
    // second index indicates by how much
    private int[] statReward1;
    private int[] statReward2;
    // first and third index indicates which stat is to be improved
    // second and fourth index indicates by how much
    private int[] statReward3;

    private int lowStatStartRange = 1;
    private int averageStatStartRange = 3;
    private int highStatStartRange = 5;
    private int veryHighStatStartRange = 8;
    private int extremeStatStartRange = 11;

    private string[] statStrings = new string[] { " max HP", " attack damage", " base armor" };

    private void Start()
    {
        icons = new InventorySlot[choiceButtons.Length];
        descriptions = new TextMeshProUGUI[choiceButtons.Length];

        for (int i = 0; i < 3; ++i)
        {
            icons[i] = choiceButtons[i].GetComponentInChildren<InventorySlot>();
            foreach (TextMeshProUGUI t in choiceButtons[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                if(t.gameObject.name.Equals("Description"))
                {
                    descriptions[i] = t;
                    break;
                }
            }
        }

        itemPool = new List<so_Item>();
        itemPool.AddRange(allItems);

        statReward1 = new int[2];
        statReward2 = new int[2];
        statReward3 = new int[4];

        DisableButtons();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (!rewardUIParent.activeSelf)
            {
                rewardUIParent.SetActive(true);
                RollNewItemRewards();
                RollStatReward();
                EnableButtons();
            }
        }
    }

    public void PickReward(int whichReward)
    {
        DisableButtons();
        if(whichReward == 0)
        {
            if (item1 != null && item1.itemType != ItemType.NULL)
            {
                PlayerManager.instance.playerInventory.Add(item1);
                itemPool.Remove(item1);
            }

            ApplyStatReward(0);
        }
        else if(whichReward == 1)
        {
            if (item1 != null && item1.itemType != ItemType.NULL)
            {
                PlayerManager.instance.playerInventory.Add(item2);
                itemPool.Remove(item2);
            }

            ApplyStatReward(1);
        }
        else
        {
            ApplyStatReward(2);
        }

        item1 = nullItem;
        item2 = nullItem;
        icons[0].AddItem(item1);
        icons[1].AddItem(item2);
        rewardUIParent.SetActive(false);

        Debug.Log("Healing for 20 after picking reward!");
        PlayerManager.instance.player.GetComponent<PlayerController>().Heal(20);
    }

    private void RollNewItemRewards()
    {
        int firstRanNum = 0;
        // If no items left assign dummy items to rewards
        // If 1 item left assign that to reward 1 and then assign dummy item to reward 2
        // Else just assign random items to the rewards
        if (itemPool.Count < 1)
        {
            item1 = nullItem;
            item2 = nullItem;
        }
        else
        {
            do
            {
                firstRanNum = Random.Range(0, itemPool.Count);
            } while (itemPool[firstRanNum] == null);

            item1 = itemPool[firstRanNum];
            //allItems[ranNum] = null;

            if (itemPool.Count < 2)
            {
                item2 = nullItem;
            }
            else
            {
                int secondRanNum = 0;
                do
                {
                    secondRanNum = Random.Range(0, itemPool.Count);
                } while (itemPool[secondRanNum] == null || firstRanNum == secondRanNum);

                item2 = itemPool[secondRanNum];
                //allItems[ranNum] = null;
            }
        }

        icons[0].AddItem(item1);
        icons[1].AddItem(item2);
    }

    private void RollStatReward()
    {
        // assume no extra stat award was gained
        statReward1[0] = -1;
        statReward2[0] = -1;
        statReward3[2] = -1;

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

                descriptions[i].SetText("and\n" + actualStatValue + statStrings[whichStat]);

                switch (i)
                {
                    case 0:
                        statReward1[0] = whichStat;
                        statReward1[1] = actualStatValue;
                        break;
                    case 1:
                        statReward2[0] = whichStat;
                        statReward2[1] = actualStatValue;
                        break;
                    default:
                        Debug.LogError("Error with assigning stat rewards to items.");
                        break;
                }
            }
            else
            {
                descriptions[i].SetText("");
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

        string tempStr = actualStatValue + statStrings[whichStat];
        statReward3[0] = whichStat;
        statReward3[1] = actualStatValue;

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
            whichStat = Random.Range(0, 3);

            tempStr += "\nand\n" + actualStatValue + statStrings[whichStat];
            statReward3[2] = whichStat;
            statReward3[3] = actualStatValue;
        }

        descriptions[2].SetText(tempStr);
    }

    private void ApplyStatReward(int whichReward)
    {
        if(whichReward == 0)
        {
            switch(statReward1[0])
            {
                case 0:
                    playerStats.UpdateMaxHp(statReward1[1]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(statReward1[1]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(statReward1[1]);
                    break;
                default:
                    break;
            }
        }
        else if(whichReward == 1)
        {
            switch (statReward2[0])
            {
                case 0:
                    playerStats.UpdateMaxHp(statReward2[1]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(statReward2[1]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(statReward2[1]);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (statReward3[0])
            {
                case 0:
                    playerStats.UpdateMaxHp(statReward3[1]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(statReward3[1]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(statReward3[1]);
                    break;
                default:
                    break;
            }

            switch (statReward3[2])
            {
                case 0:
                    playerStats.UpdateMaxHp(statReward3[3]);
                    break;
                case 1:
                    playerStats.UpdateBaseDamage(statReward3[3]);
                    break;
                case 2:
                    playerStats.UpdateBaseArmor(statReward3[3]);
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
            statValue = Random.Range(lowStatStartRange, averageStatStartRange);
        }
        else if (randomNum < 85)
        {
            statValue = Random.Range(averageStatStartRange, highStatStartRange);
        }
        else if (randomNum < 95)
        {
            statValue = Random.Range(highStatStartRange, veryHighStatStartRange);
        }
        else if (randomNum < 99)
        {
            statValue = Random.Range(veryHighStatStartRange, extremeStatStartRange);
        }
        else
        {
            statValue = Random.Range(extremeStatStartRange, extremeStatStartRange + 2);
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
