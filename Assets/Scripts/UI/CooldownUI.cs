using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public Image leftClickCooldownImage;
    public Image rightClickCooldownImage;

    public Image leftClickImage;
    public Image rightClickImage;

    private void Start()
    {
        leftClickCooldownImage.fillAmount = 0;
        rightClickCooldownImage.fillAmount = 0;
    }

    public void UpdateSliders(float leftClickCD, float rightClickCD)
    {
        leftClickCooldownImage.fillAmount = leftClickCD;
        rightClickCooldownImage.fillAmount = rightClickCD;
    }

    public void UpdateImages(Sprite sprite, bool isLeftImage)
    {
        if (isLeftImage)
        {
            leftClickImage.sprite = sprite;
        }
        else
        {
            rightClickImage.sprite = sprite;
        }
    }

    public void UpdateImages(Sprite left, Sprite right)
    {
        leftClickImage.sprite = left;
        rightClickImage.sprite = right;
    }
}
