using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_PlankCondition : Gr_BaseConditionInteractable
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void ResolveConditions()
    {
        rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
