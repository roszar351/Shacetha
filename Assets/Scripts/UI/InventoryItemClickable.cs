using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemClickable : MonoBehaviour, IPointerClickHandler
{
    public InventorySlot mySlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            mySlot.UseItem(true);
        else if (eventData.button == PointerEventData.InputButton.Right)
            mySlot.UseItem(false);
    }
}
