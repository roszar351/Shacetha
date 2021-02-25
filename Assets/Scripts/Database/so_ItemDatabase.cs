using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "itemDB_", menuName = "Database/Item Database")]
public class so_ItemDatabase : ScriptableObject
{
    public List<so_Item> allItems;
}
