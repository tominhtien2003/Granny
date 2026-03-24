using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Pet_Shop_Item_Data", menuName = "Data/Pet Shop Item Data")]
public class PetShopListItemData_SO : ScriptableObject
{
    public List<PetItem> m_itemData;
}
