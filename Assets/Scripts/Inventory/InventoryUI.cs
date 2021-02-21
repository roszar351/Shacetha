using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject characterSheetUI;
    public Transform itemsParent;

    Inventory _inventory;
    InventorySlot[] _slots;

    void Start()
    {
        _inventory = PlayerManager.instance.playerInventory;
        _inventory.onItemChangedCallBack += UpdateUI;
        _slots = itemsParent.GetComponentsInChildren<InventorySlot>();

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

        for (int i = 0; i < _slots.Length; ++i)
        {
            if (i < _inventory.items.Count)
            {
                _slots[i].AddItem(_inventory.items[i]);
            }
            else
            {
                _slots[i].ClearSlot();
            }
        }
    }
}
