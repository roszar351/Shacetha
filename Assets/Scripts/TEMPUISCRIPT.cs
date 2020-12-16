using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPUISCRIPT : MonoBehaviour
{
    public GameObject inventory;
    public GameObject characterSheet;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(!inventory.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            characterSheet.SetActive(!characterSheet.activeSelf);
        }
    }
}
