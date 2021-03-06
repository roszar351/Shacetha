using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Database : MonoBehaviour
{
    public so_ItemDatabase normalItems;
    public so_ItemDatabase rareItems;
    public so_ItemDatabase epicItems;
    public so_ItemDatabase specialItems;

    private List<so_ItemDatabase> _databases;
    
    private static Database instance;

    private void Awake()
    {
        _databases.Add(normalItems);
        _databases.Add(rareItems);
        _databases.Add(epicItems);
        _databases.Add(specialItems);
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Tried to create another instance of " + this.name);
            return;
        }
    }

    public so_Item GetItemByID(string ID)
    {
        foreach (var db in _databases)
        {
            so_Item item = db.allItems.FirstOrDefault(x => x.itemID == ID);
            if (item != null)
                return item;
        }

        return null;
    }
    
    // Alternative to looking by ID but ID should be prefered
    public so_Item GetItemByName(string name)
    {
        foreach (var db in _databases)
        {
            so_Item item = db.allItems.FirstOrDefault(x => x.name == name);
            if (item != null)
                return item;
        }

        return null;
    }

    public so_Item GetRandomItem()
    {
        if (_databases.Count == 0)
            return null;
        
        int randomIndex = Random.Range(0, _databases.Count);
        int firstCheck = randomIndex;
        while (_databases[randomIndex].allItems.Count == 0)
        {
            randomIndex += 1 % _databases.Count;
            // Checked all dbs and all were empty
            if (randomIndex == firstCheck)
                return null;
        }
        return _databases[randomIndex].allItems[Random.Range(0, _databases[randomIndex].allItems.Count)];
    }

    // rarity, 0=normal, 1=rare, 2=epic, 3=special
    public so_Item GetRandomItem(int rarity)
    {
        if (_databases.Count == 0)
            return null;
        
        while (_databases[rarity].allItems.Count == 0)
        {
            rarity--;
            
            // Checked all dbs and all were empty
            if (rarity < 0)
                return null;
        }
        return _databases[rarity].allItems[Random.Range(0, _databases[rarity].allItems.Count)];
    }

    public List<so_Item> GetAllItems(int rarity)
    {
        return rarity > _databases.Count - 1 ? null : _databases[rarity].allItems;
    }
}
