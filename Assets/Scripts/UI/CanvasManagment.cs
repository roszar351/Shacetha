using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManagment : MonoBehaviour
{
    [SerializeField] private GameObject screen1;
    [SerializeField] private GameObject screen2;
    [SerializeField] private GameObject screen3;

    public void GoToScreen1()
    {
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
    }
    
    public void GoToScreen2()
    {
        screen2.SetActive(true);
        screen1.SetActive(false);
        screen3.SetActive(false);
    }
    
    public void GoToScreen3()
    {
        screen3.SetActive(true);
        screen2.SetActive(false);
        screen1.SetActive(false);
    }
}
