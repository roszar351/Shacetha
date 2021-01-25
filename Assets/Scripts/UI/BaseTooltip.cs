using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Base class that will activate the tooltiptext and box and will follow the mouse while active
// Has a virtual display and close tooltip to customise what is displayed and what happens when closing tooltip
public class BaseTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform tooltipParent;
    public Image tooltipBackground;
    public TextMeshProUGUI tooltipText;

    protected bool isTooltipActive;

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Debug.Log("Cursor Entering " + name + " GameObject");
        isTooltipActive = true;
        tooltipParent.transform.position = pointerEventData.position;
        DisplayTooltip(pointerEventData);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Debug.Log("Cursor Exiting " + name + " GameObject");
        isTooltipActive = false;
        CloseTooltip(pointerEventData);
    }

    // Override this to specify what text should be displayed
    public virtual void DisplayTooltip(PointerEventData pointerEventData)
    {
        Color color = Random.ColorHSV();
        Debug.Log("Override DisplayTooltip!");

        tooltipText.SetText("Override DisplayTooltip!");

        tooltipBackground.color = color;

        tooltipParent.gameObject.SetActive(true);
        tooltipParent.transform.position = pointerEventData.position;
    }

    public virtual void CloseTooltip(PointerEventData pointerEventData)
    {
        Debug.Log("Override CloseTooltip!");
        tooltipParent.gameObject.SetActive(false);
    }
}
