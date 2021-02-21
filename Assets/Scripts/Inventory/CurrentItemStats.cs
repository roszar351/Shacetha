using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentItemStats : MonoBehaviour
{
    public so_NPCStats myCharStats;
    private so_Item _myStats;

    public void SetStats(so_Item item)
    {
        _myStats = item;
    }

    public int GetModifierValue()
    {
        return _myStats.modifierValue;
    }
}
