using System;
using System.Collections.Generic;
using UnityEngine;
public enum Adventure_ItemType
{
    HandItem,   
    Buff,       
    Key         
}

[CreateAssetMenu(fileName = "SchoolEscape_ITEM",menuName = "Data/SchoolEscape/ItemDatabase")]
public class Adventure_ItemDatabase : ScriptableObject
{
    public List<Adventure_ItemInfo> items;

    private Dictionary<string, Adventure_ItemInfo> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, Adventure_ItemInfo>();
        foreach (var item in items)
        {
            if (!lookup.ContainsKey(item.itemName))
                lookup.Add(item.itemName, item);
        }
    }

    public Adventure_ItemInfo GetItem(string itemName)
    {
        if (lookup.TryGetValue(itemName, out var item))
            return item;
        Debug.LogWarning("Item not found: " + itemName);
        return null;
    }
}
[Serializable]
public class Adventure_ItemInfo
{
    public string itemName;
    public string describe;
    public Sprite icon;
    public Adventure_ItemBase itemPrefab;
    public bool canRoll;
    public Adventure_ItemType itemType;
}
