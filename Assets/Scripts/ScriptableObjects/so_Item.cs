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
    public float useCooldown;

    private GameObject parentObject;

    public virtual void Use()
    {
        Debug.Log("Using Item: " + name);
    }

    public override string ToString()
    {
        return name;
    }
}

public enum ItemType
{
    Weapon,
    Shield
}

