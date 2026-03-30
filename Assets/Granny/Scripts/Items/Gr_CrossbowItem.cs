using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_CrossbowItem : Gr_BaseItemInteractable, IBulletReceiver, IUsableItem
{
    [SerializeField] private Transform cylinderTrans;
    [SerializeField] private Vector3 rotationLocal;
    [SerializeField] private Vector3 positionLocal;

    private Gr_BaseItemInteractable bulletItem;
    
    public bool CanUse => bulletItem != null;
    public bool TryLoadBullet(Gr_BaseItemInteractable bullet)
    {
        if (bullet.ItemData.Type != ItemType.Cylinder) 
            return false;
        
        if (bulletItem != null) 
            return false;
        
        bulletItem = bullet;
        
        bulletItem.transform.SetParent(cylinderTrans);
        bulletItem.transform.localPosition = Vector3.zero;
        bulletItem.transform.localRotation = Quaternion.identity;
        
        Gr_EventManager.Notify(new ActivateStateShootButtonEvent(true));
        
        return true; 
    }

    public override void Interact()
    {
        base.Interact();
        if (bulletItem != null)
        {
            Gr_EventManager.Notify(new ActivateStateShootButtonEvent(true));
        }
    }

    public void Use()
    {
        if (CanUse)
        {
            bulletItem.SetupForDrop(); 
            var rb = bulletItem.GetRigidbody();
            if (rb != null)
            {
                rb.AddForce(Camera.main.transform.forward * 20f, ForceMode.Impulse); 
            }
            bulletItem = null;
            
            Gr_EventManager.Notify(new ActivateStateShootButtonEvent(false));
        }
    }

    public override void SetupForDrop()
    {
        base.SetupForDrop();
        Gr_EventManager.Notify(new ActivateStateShootButtonEvent(false));
    }
}
