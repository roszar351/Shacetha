using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "npcstats_", menuName = "NPC/Stats")]
public class so_NPCStats : ScriptableObject
{
    new public string name = "New NPC";
    public int maxHp = 100;
    public int baseDamage = 10;
    public int baseArmor = 0;
    public float movementSpeed = 1f;
    public float attackRange = 0.5f;

    // Used to increase or decrease max hp value, maxHp cant go under 1
    public void UpdateMaxHp(int value)
    {
        if(maxHp + value < 1)
        {
            maxHp = 1;
        }
        else
        {
            maxHp += value;
        }
    }

    public void UpdateBaseDamage(int value)
    {
        if(baseDamage + value < 1)
        {
            baseDamage = 1;
        }
        else
        {
            baseDamage += value;
        }
    }

    public void UpdateBaseArmor(int value)
    {
        baseArmor += value;
    }

    public void ResetStats(so_NPCStats defaultStats)
    {
        maxHp = defaultStats.maxHp;
        baseDamage = defaultStats.baseDamage;
        baseArmor = defaultStats.baseArmor;
        movementSpeed = defaultStats.movementSpeed;
        attackRange = defaultStats.attackRange;
    }
}
