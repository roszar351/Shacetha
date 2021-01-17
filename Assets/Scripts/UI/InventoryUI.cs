﻿using System.Collections;
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
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        if (Input.GetButtonDown("Character Sheet"))
        {
            characterSheetUI.SetActive(!characterSheetUI.activeSelf);
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
