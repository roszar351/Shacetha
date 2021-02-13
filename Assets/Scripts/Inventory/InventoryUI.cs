using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject characterSheetUI;
    public Transform itemsParent;

    Inventory inventory;
    InventorySlot[] slots;

    void Start()
    {
        inventory = PlayerManager.instance.playerInventory;
        inventory.onItemChangedCallBack += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // Make sure to display any items already present.
        UpdateUI();

        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if(inventoryUI.activeSelf)
            {
                AudioManager.instance.PlayOneShotSound("CloseUI");
                inventoryUI.SetActive(false);
            }
            else
            {
                AudioManager.instance.PlayOneShotSound("OpenUI");
                inventoryUI.SetActive(true);
            }
        }
        if (Input.GetButtonDown("Character Sheet"))
        {
            if (characterSheetUI.activeSelf)
            {
                AudioManager.instance.PlayOneShotSound("CloseUI");
                characterSheetUI.SetActive(false);
            }
            else
            {
                AudioManager.instance.PlayOneShotSound("OpenUI");
                characterSheetUI.SetActive(true);
            }  
        }
    }

    void UpdateUI()
    {
        //Debug.Log("UPDATING UI!");

        for (int i = 0; i < slots.Length; ++i)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
