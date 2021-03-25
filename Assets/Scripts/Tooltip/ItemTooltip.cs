using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : BaseTooltip
{
    private InventorySlot _invSlot;

    private void Start()
    {
        _invSlot = gameObject.GetComponent<InventorySlot>();
    }

    void Update()
    {
        if (isTooltipActive)
        {
            SetItemText();
        }
    }

    public override void DisplayTooltip(PointerEventData pointerEventData)
    {
        CheckTooltipParts();
        SetItemText();
    }

    public override void CloseTooltip(PointerEventData pointerEventData)
    {
        base.CloseTooltip(pointerEventData);
    }

    private void SetItemText()
    {
        string tempStr;
        if (_invSlot != null)
        {
            tempStr = _invSlot.ToString();
            if (tempStr != null)
            {
                tooltipText.SetText(tempStr);
                tooltipParent.gameObject.SetActive(true);
            }
            else
            {
                tooltipParent.gameObject.SetActive(false);
            }
        }
    }
}

