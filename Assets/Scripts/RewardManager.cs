using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardUIParent;
    public GameObject[] choiceButtons;
    public List<so_Item> allItems; //TODO: will have to change how this item list will work as currently all items are dragged in through editor and picked randomly

    private List<so_Item> itemPool;
    private InventorySlot[] icons;
    private TextMeshProUGUI[] descriptions;

    private so_Item item1;
    private so_Item item2;

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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            rewardUIParent.SetActive(!rewardUIParent.activeSelf);
        }

        // TODO: REMOVE "for testing"
        if(Input.GetKeyDown(KeyCode.H))
        {
            //TODO: Reroll reward for testing
            RollNewItemRewards();
            RollStatReward();
        }
    }

    private void RollNewItemRewards()
    {
        int firstRanNum = 0;
        // If no items left assign dummy items to rewards
        // If 1 item left assign that to reward 1 and then assign dummy item to reward 2
        // Else just assign random items to the rewards
        if (itemPool.Count < 1)
        {
            //TODO: make an empty item if no items left
            //item1 = nullItem;
            //item2 = nullItem;
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
                //TODO: make an empty item if no items left
                //item2 = nullItem;
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
        //TODO: Finish rolling stat rewards
        // odds for additional stat reward with items
        // 5% for negative stat reward
        // 10% for positive stat reward
        // 85% for no stat reward

        // odds for additional stat reward
        // 10% for additional stat reward
        // 20% for additional stat reward to be negative
        // 80% for additional stat reward to be positive

        // odds for level of stat reward
        // very low  - 5%
        // low       - 10%
        // average   - 65%
        // high      - 15%
        // very high - 5%
    }
}
