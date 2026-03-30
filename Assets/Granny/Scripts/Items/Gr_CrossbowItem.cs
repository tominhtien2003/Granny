using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_CrossbowItem : Gr_BaseItemInteractable
{
    [SerializeField] private Transform cylinderTrans;
    [SerializeField] private Vector3 rotationLocal;
    [SerializeField] private Vector3 positionLocal;

    private Gr_BaseItemInteractable cylenderItem;

    private void OnEnable()
    {
        Gr_EventManager.AddListener<CylinderInteractWithCrossbowEvent>(SetupHolderForCylinder);
    }

    private void OnDisable()
    {
        Gr_EventManager.RemoveListener<CylinderInteractWithCrossbowEvent>(SetupHolderForCylinder);
    }

    public override void SetupForHold(Transform handPoint)
    {
        base.SetupForHold(handPoint);
        if (cylenderItem != null)
        {
            Gr_EventManager.Notify(new DropCrossbowEvent(cylenderItem));
        }
    }

    public override void SetupForDrop()
    {
        base.SetupForDrop();
        if (cylenderItem != null)
        {
            Gr_EventManager.Notify(new DropCrossbowEvent(null));
        }
    }

    private void SetupHolderForCylinder(CylinderInteractWithCrossbowEvent e)
    {
        cylenderItem = e.obj;
        if (e.obj == null) return;
        cylenderItem.transform.SetParent(cylinderTrans);
        cylenderItem.transform.localPosition = Vector3.zero;
        cylenderItem.transform.localRotation = Quaternion.identity;
    }
}
