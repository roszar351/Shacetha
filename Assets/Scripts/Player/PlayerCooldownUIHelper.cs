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

    public event System.Action<so_Item, bool> OnEquippedItemChanged;

    private float _maxLeftCd = 1;
    private float _maxRightCd = 1;

    public CooldownUI ui;

    public void UpdateCooldowns(float left, float right)
    {
        if (left < 0)
            left = 0;
        if (right < 0)
            right = 0;

        ui.UpdateSliders(left / _maxLeftCd, right / _maxRightCd);
    }

    public void ChangeItem(so_Item item, bool isLeft)
    {
        if (OnEquippedItemChanged != null)
            OnEquippedItemChanged(item, isLeft);
    }

    public void SetMaxCooldowns(float maxLeft, float maxRight)
    {
        _maxLeftCd = maxLeft;
        _maxRightCd = maxRight;
    }

    public void SetMaxLeftCooldown(float maxLeft)
    {
        _maxLeftCd = maxLeft;
    }

    public void SetMaxRightCooldown(float maxRight)
    {
        _maxRightCd = maxRight;
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
