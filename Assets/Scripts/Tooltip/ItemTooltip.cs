using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : BaseTooltip
{
    private InventorySlot invSlot;

    private void Start()
    {
        tooltipBackground = tooltipParent.gameObject.GetComponentInChildren<Image>();
        tooltipText = tooltipParent.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        invSlot = gameObject.GetComponent<InventorySlot>();
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
        SetItemText();
    }

    public override void CloseTooltip(PointerEventData pointerEventData)
    {
        base.CloseTooltip(pointerEventData);
    }

    private void SetItemText()
    {
        string tempStr;
        if (invSlot != null)
        {
            tempStr = invSlot.ToString();
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

