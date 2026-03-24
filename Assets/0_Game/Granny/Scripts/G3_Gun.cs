using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_Gun : Adventure_InteractObj
{
    private bool picked = false;
    public override void Interact()
    {
        base.Interact();
        picked = true;
        G3_Manager.Instance.EnableGunMode();
        gameObject.SetActive(false);
    }
}
