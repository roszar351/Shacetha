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
}
