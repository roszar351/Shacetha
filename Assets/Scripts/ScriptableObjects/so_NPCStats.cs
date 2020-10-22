using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Stats", menuName = "NPC/Stats")]
public class so_NPCStats : ScriptableObject
{
    public int maxHp = 100;
    public int baseDamage = 10;
    public int baseArmor = 0;
    public float movementSpeed = 1f;
    public float attackRange = 0.5f;
}
