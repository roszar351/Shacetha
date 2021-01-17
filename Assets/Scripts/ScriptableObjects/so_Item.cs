using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item_", menuName = "Inventory/Item")]
public class so_Item : ScriptableObject
{
    new public string name = "New Item";
    public ItemType itemType;
    public Sprite weaponSprite = null;
    public int modifierValue; // If item is a shield it will reduce incoming damage by this value, if its a weapon it will do this much damage
    public float itemRange;
    public float damageRadius;
    public float useCooldown;
    public float duration = 1f;

    public override string ToString()
    {
        return name;
    }
}

// TODO: can add different item types such as spear weapon, aoe weapon that function differently
public enum ItemType
{
    Sword,
    Spear,
    Dagger,
    Axe,
    Shield
}

