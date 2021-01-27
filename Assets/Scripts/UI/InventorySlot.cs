using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    so_Item item;

    public void AddItem(so_Item newItem)
    {
        item = newItem;
        if (newItem.itemType != ItemType.NULL)
        {
            icon.sprite = item.weaponSprite;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
        if(removeButton != null)
            removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        if (item != null && item.itemType == ItemType.NULL)
        {
            Debug.LogError("Attempted to remove a null item!");
            return;
        }

        item = null;
        icon.sprite = null;
        icon.enabled = false;
        if (removeButton != null)
            removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        if (item.itemType == ItemType.NULL)
        {
            Debug.LogError("Attempted to remove a null item!");
            return;
        }

        PlayerManager.instance.playerInventory.Remove(item);
    }

    public void UseItem(bool isLeft)
    {
        if (item.itemType == ItemType.NULL)
        {
            Debug.LogError("Attempted to use a null item!");
            return;
        }

        if (item != null)
        {
            if (isLeft)
            {
                PlayerManager.instance.playerHands.EquipItem(item, true);
                OnRemoveButton();
            }
            else
            {
                PlayerManager.instance.playerHands.EquipItem(item, false);
                OnRemoveButton();
            }
        }
    }

    public override string ToString()
    {
        if (item == null || item.itemType == ItemType.NULL)
        {
            return null;
        }
        else
        {
            return item.ToString();
        }
    }
}
