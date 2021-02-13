using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentItemStats : MonoBehaviour
{
    private so_Item myStats;

    public void SetStats(so_Item item)
    {
        myStats = item;
    }

    public int GetModifierValue()
    {
        return myStats.modifierValue;
    }
}
