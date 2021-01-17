using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCooldownUIHelper : MonoBehaviour
{
    #region Singleton
    public static PlayerCooldownUIHelper instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Tried to create another instance of " + this.name);
        }
    }
    #endregion

    private float maxLeftCD = 1;
    private float maxRightCD = 1;

    public CooldownUI ui;

    public void UpdateCooldowns(float left, float right)
    {
        if (left < 0)
            left = 0;
        if (right < 0)
            right = 0;

        ui.UpdateSliders(left / maxLeftCD, right / maxRightCD);
    }

    public void SetMaxCooldowns(float maxLeft, float maxRight)
    {
        maxLeftCD = maxLeft;
        maxRightCD = maxRight;
    }

    public void SetMaxLeftCooldown(float maxLeft)
    {
        maxLeftCD = maxLeft;
    }

    public void SetMaxRightCooldown(float maxRight)
    {
        maxRightCD = maxRight;
    }

    public void ChangeImages(Sprite sprite, bool isLeft)
    {
        ui.UpdateImages(sprite, isLeft);
    }

    public void ChangeImages(Sprite left, Sprite right)
    {
        ui.UpdateImages(left, right);
    }
}
