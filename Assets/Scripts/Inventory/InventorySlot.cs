using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    so_Item _item;

    public void AddItem(so_Item newItem)
    {
        _item = newItem;
        if (newItem.itemType != ItemType.NULL)
        {
            icon.sprite = _item.weaponSprite;
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
        if (_item != null && _item.itemType == ItemType.NULL)
        {
            Debug.LogError("Attempted to remove a null item!");
            return;
        }

        _item = null;
        icon.sprite = null;
        icon.enabled = false;
        if (removeButton != null)
            removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        if (_item.itemType == ItemType.NULL)
        {
            Debug.LogError("Attempted to remove a null item!");
            return;
        }

        PlayerManager.instance.playerInventory.Remove(_item);
    }

    public void UseItem(bool isLeft)
    {
        if (_item.itemType == ItemType.NULL)
        {
            Debug.LogError("Attempted to use a null item!");
            return;
        }

        if (_item != null)
        {
            AudioManager.instance.PlayOneShotSound("EquipItem");
            if (isLeft)
            {
                PlayerManager.instance.playerHands.EquipItem(_item, true);
                OnRemoveButton();
            }
            else
            {
                PlayerManager.instance.playerHands.EquipItem(_item, false);
                OnRemoveButton();
            }
        }
    }

    public override string ToString()
    {
        if (_item == null || _item.itemType == ItemType.NULL)
        {
            return null;
        }
        else
        {
            return _item.ToString();
        }
    }
}
