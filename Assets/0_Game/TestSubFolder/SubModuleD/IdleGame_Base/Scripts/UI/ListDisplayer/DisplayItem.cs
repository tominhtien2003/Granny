using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class DisplayItem
{
    public Sprite m_displayIcon;
}
[System.Serializable]
public class ShopItem: DisplayItem
{
    public string m_itemName;
    public double m_itemPrice;
    public int m_itemAdAmount;
}
[System.Serializable]
public class PetItem: ShopItem
{
    public double m_baseMultiplier;
    public Mesh m_mesh;
    public Material[] m_mats;
    public Material m_trailMat;
    public int m_rarityIndex;
    public int m_id;
    public int m_gemPrice;
}
[System.Serializable]
public class InventoryPetItem: DisplayItem
{
    public PetItem m_petItem;
    public int m_level;
    public int m_amount;
    public double m_currentMultiplier;

    public InventoryPetItem(PetItem petItem, int level = 0, int amount = 1)
    {
        m_petItem = petItem;
        m_amount = amount;
        m_displayIcon = petItem.m_displayIcon;
        m_level = level;
        //m_currentMultiplier = petItem.m_multiplierPerLevel[level];
        m_currentMultiplier = petItem.m_baseMultiplier * (1 + 0.2f * level);
    }
}