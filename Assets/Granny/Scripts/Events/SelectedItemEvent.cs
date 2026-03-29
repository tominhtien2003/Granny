using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SelectedItemEvent
{
    public Gr_IInteractable obj;
    public SelectedItemEvent(Gr_IInteractable obj)
    {
        this.obj = obj;
    }
}
