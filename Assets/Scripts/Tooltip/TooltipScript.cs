using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipScript : MonoBehaviour
{
    public RectTransform backgroundRect;
    public RectTransform textRect;

    private Rect _screenRect;

    private void Start()
    {
        _screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position = Input.mousePosition;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                gameObject.SetActive(false);
                return;
            }
            //TODO: currently only check if tooltip is on screen towards the bottom, need to add a check for left side(as tooltip always shows up on left
            // which prevents it from going off screen on right or up, also dont use hard coded value of 100f but currently its fine as tooltip doesn't dynamicaly resize to fit text
            Vector3 down = transform.position - new Vector3(0, 100f, 0);
            _screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
            if (!_screenRect.Contains(down))
            {
                Debug.Log("OUTSIDE");
                backgroundRect.localPosition = new Vector3(-25, 35, 0);
                textRect.localPosition = new Vector3(-25, 35, 0);
            }
            else
            {
                backgroundRect.localPosition = new Vector3(-25, -35, 0);
                textRect.localPosition = new Vector3(-25, -35, 0);
            }
        }
    }
}
