using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Gr_BaseItemInteractable : Gr_InteractableBase
{
    [SerializeField] private Collider col;
    protected Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void OnFocusEnter()
    {
        
    }
    public override void Interact()
    {
        Gr_PlayerHolder.Instance.Hold(this);
    }
    public override void OnFocusExit()
    {

    }
    public virtual void SetupForHold(Transform handPoint)
    {
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(handPoint);

        transform.localScale *= 2f;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public virtual void SetupForDrop()
    {
        transform.localScale /= 2;
        transform.SetParent(null);
        rb.isKinematic = false; 
        col.enabled = true;
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
