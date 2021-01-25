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
        icon.sprite = item.weaponSprite;
        icon.enabled = true;
        if(removeButton != null)
            removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        if (removeButton != null)
            removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        PlayerManager.instance.playerInventory.Remove(item);
    }

    public void UseItem(bool isLeft)
    {
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
        if (item == null)
        {
            return null;
        }
        else
        {
            return item.ToString();
        }
    }
}
