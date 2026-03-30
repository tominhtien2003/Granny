using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Gr_BaseItemInteractable : Gr_InteractableBase
{
    public GameObject glow;
    public SOItemData ItemData;
    public Collider col;
    protected Rigidbody rb;
    protected Vector3 rootScale;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rootScale = transform.localScale;
        if (glow != null) glow.SetActive(true);
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
        if (glow != null) glow.SetActive(false);
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(handPoint);

        //transform.localScale *= 2f;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public virtual void SetupForDrop()
    {
        if (glow!=null) glow.SetActive(true);
        //transform.localScale = rootScale;
        transform.SetParent(null);
        rb.isKinematic = false; 
        col.enabled = true;
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
