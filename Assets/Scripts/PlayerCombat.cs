using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for handling comabat related input for the player
 * 
 */
public class PlayerCombat : MonoBehaviour
{
    private TextPopup temp1 = null;
    private TextPopup temp2 = null;

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Debug.Log("Left Attack!");
            if (temp1 == null)
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouse.z = 0;
                temp1 = TextPopup.Create(mouse, "Left!", Color.green);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Right Attack!");
            if (temp2 == null)
            { 
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouse.z = 0;
                temp2 = TextPopup.Create(mouse, "Right!", Color.red);
            }
        }
    }
}
