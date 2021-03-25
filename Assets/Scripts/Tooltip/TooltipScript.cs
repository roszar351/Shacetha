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
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                backgroundRect.gameObject.SetActive(false);
                textRect.gameObject.SetActive(false);
                gameObject.SetActive(false);
                return;
            }
            backgroundRect.gameObject.SetActive(true);
            textRect.gameObject.SetActive(true);
            transform.position = Input.mousePosition;
            // this prevents it from going off screen on right or up, also in future dont use hard coded value of 115f
            // but currently its fine as tooltip doesn't dynamicaly resize to fit text
            Vector3 down = transform.position - new Vector3(0, 115f, 0);
            Vector3 left = transform.position - new Vector3(115f, 0, 0);
            _screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
            float leftMult = 1;
            float downMult = 1;
            if (!_screenRect.Contains(down))
            {
                //Debug.Log("OUTSIDE BOTTOM");
                downMult = -1;
            }
            if (!_screenRect.Contains(left))
            {
                //Debug.Log("OUTSIDE LEFT");
                leftMult = -1.2f;
            }
            backgroundRect.localPosition = new Vector3(-50 * leftMult, -50 * downMult, 0);
            textRect.localPosition = new Vector3(-50 * leftMult, -50 * downMult, 0);
        }
    }
}
