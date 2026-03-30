using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_CylinderItem : Gr_BaseItemInteractable
{
    public override void Interact()
    {
        var currentItem = Gr_PlayerHolder.Instance.GetCurrentItem();
        if (currentItem != null && currentItem is IBulletReceiver receiver)
        {
            bool isLoadedSuccess = receiver.TryLoadBullet(this);
            
            if (isLoadedSuccess)
            {
                SetUpForLoaded();
            }
            else
            {
                Gr_PlayerHolder.Instance.Hold(this);
            }
        }
        else
        {
            Gr_PlayerHolder.Instance.Hold(this);
        }
    }
    private void SetUpForLoaded()
    {
        rb.isKinematic = true;
        if (glow != null) glow.SetActive(false);
        col.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public override void SetupForDrop()
    {
        base.SetupForDrop();
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
}
