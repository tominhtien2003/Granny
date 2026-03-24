using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_ItemKey : Adventure_ItemBase
{
    
    public override void Use()
    {
        OnUnequip();
        Adventure_InventoryManager.Instance.DeselectItem();
        Adventure_InventoryManager.Instance.RemoveItemFromInstance(this);
    }
}
