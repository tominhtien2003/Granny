using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Shop_Item_Data", menuName = "Data/Shop Item Data")]
public class ShopListItemData_SO : ScriptableObject
{
    public List<ShopItem> m_itemData;
}
