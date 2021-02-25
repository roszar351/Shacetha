using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ToggleOnEntry : MonoBehaviour
{
    [SerializeField]
    private ToggleMode toggleMode;

    [SerializeField]
    private int colliderLayer = 8;
    
    [SerializeField]
    private bool destroySelfAfterToggle;
    
    [SerializeField]
    private GameObject firstObject;
    
    [SerializeField]
    private GameObject secondObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == colliderLayer)
        {
            switch (toggleMode)
            {
                case ToggleMode.ToggleOneObject:
                    firstObject.SetActive(!firstObject.activeSelf); 
                    break;
                case ToggleMode.ToggleBetweenTwoObjects:
                    firstObject.SetActive(!firstObject.activeSelf); 
                    secondObject.SetActive(!secondObject.activeSelf); 
                    break;
                default:
                    Debug.LogError("Unknown ToggleMode: " + toggleMode);
                    break;
            }
            if(destroySelfAfterToggle)
                Destroy(this);
        }
    }
}

public enum ToggleMode
{
    ToggleOneObject,
    ToggleBetweenTwoObjects
}
