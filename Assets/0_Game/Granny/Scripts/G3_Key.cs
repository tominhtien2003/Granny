using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_Key : Adventure_InteractObj
{
    [SerializeField] GameObject atticDoorArrow, startArrow;
    public override void Interact()
    {
        base.Interact();
        Adventure_InventoryManager.Instance.AddItem(Adventure_InventoryManager.Instance.itemDatabase.items[0].itemName);
        G3_UIManager.Instance.noti2.gameObject.SetActive(false);
        gameObject.SetActive(false);
        atticDoorArrow.SetActive(true);
        startArrow.SetActive(false);
        G3_UIManager.Instance.SetUIWarning(false);
    }
}
