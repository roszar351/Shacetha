using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialObject;

    private void LateUpdate()
    {
        if (tutorialObject.activeSelf)
        {
            tutorialObject.transform.position =
                PlayerManager.instance.player.transform.position - new Vector3(3f, -1.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            tutorialObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            tutorialObject.SetActive(false);
        }
    }
}
