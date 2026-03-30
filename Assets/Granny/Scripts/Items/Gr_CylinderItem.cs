using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_CylinderItem : Gr_BaseItemInteractable
{
    public override void Interact()
    {
        var currentItem = Gr_PlayerHolder.Instance.GetCurrentItem();
        if (currentItem != null && currentItem.ItemData.Type == ItemType.Crossbow)
        {
            SetUpForCrossbow();
            Gr_EventManager.Notify(new CylinderInteractWithCrossbowEvent(this));
        }
        else
        {
            Gr_PlayerHolder.Instance.Hold(this);
        }
    }

    private void SetUpForCrossbow()
    {
        rb.isKinematic = true;
        if (glow != null) glow.SetActive(false);
        col.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
