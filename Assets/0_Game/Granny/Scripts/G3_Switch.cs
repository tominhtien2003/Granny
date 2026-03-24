using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_Switch : Adventure_InteractObj
{
    [SerializeField] G3_Zipline zipline;

    public override void Interact()
    {
        base.Interact();
        zipline.Active();
    }
    public void ResetSwitch()
    {
        _triggerBox.gameObject.SetActive(true);
    }
}
