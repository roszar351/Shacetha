using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;

    public int space = 20;
    public List<so_Item> items = new List<so_Item>();

    public bool Add(so_Item item)
    {
        if (items.Count >= space)
        {
            Debug.LogWarning("Full inventory, no room to pick up: " + item.name);

            return false;
        }

        items.Add(item);

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();

        return true;
    }

    public void Remove(so_Item item)
    {
        items.Remove(item);

        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }
}
