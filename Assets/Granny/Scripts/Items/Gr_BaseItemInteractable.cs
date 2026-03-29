using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Gr_BaseItemInteractable : Gr_InteractableBase
{
    public SOItemData ItemData;
    [SerializeField] private Collider col;
    protected Rigidbody rb;
    protected Vector3 rootScale;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rootScale = transform.localScale;
    }
    public override void OnFocusEnter()
    {
        Gr_EventManager.Notify(new FocusItemEvent(ItemData.Description));
    }
    public override void Interact()
    {
        Gr_PlayerHolder.Instance.Hold(this);
    }
    public override void OnFocusExit()
    {
        Gr_EventManager.Notify(new FocusItemEvent(""));
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
        transform.localScale = rootScale;
        transform.SetParent(null);
        rb.isKinematic = false; 
        col.enabled = true;
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
