using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHelperScript : MonoBehaviour
{
    [SerializeField] private so_GameEvent eventToFire;
    [SerializeField] private int howManyTimes = 1;
    [SerializeField] private int howManyEnemies = 3;
    [SerializeField] private GameObject insideOutsideController;
    [SerializeField] private GameObject gate;

    public void FireEvent()
    {
        if (howManyTimes <= 0)
        {
            //Debug.LogError("Event already raised");
            return;
        }
        
        if (howManyEnemies != 1)
        {
            howManyEnemies--;
            return;
        }

        howManyTimes--;
        insideOutsideController.SetActive(true);
        gate.SetActive(false);
        eventToFire.Raise();
    }
}
