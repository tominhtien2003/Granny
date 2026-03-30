using UnityEngine;

public class Gr_PadlockCondition : Gr_BaseConditionInteractable
{
    [SerializeField] Rigidbody rbRelateObj;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected override void ResolveConditions()
    {
        rbRelateObj.isKinematic = false;
        rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
