using UnityEngine;

public class Gr_PlayerHolder : MonoBehaviour
{
    public static Gr_PlayerHolder Instance;
    [SerializeField] private Transform hand;
    [SerializeField] private float dropForce = 10f;
    [SerializeField] private float dropUpwardForce = 1.5f;

    private Gr_BaseItemInteractable currentItem;
    private Camera mainCam;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mainCam = Camera.main;
    }
    public void Hold(Gr_BaseItemInteractable newItem)
    {
        if (currentItem != null)
        {
            Drop();
        }
        currentItem = newItem;
        currentItem.SetupForHold(hand);
        Gr_EventManager.Notify(new SelectedItemEvent(currentItem));
    }
    public void Drop()
    {
        if (currentItem == null) return;
        currentItem.SetupForDrop();

        var rb = currentItem.GetRigidbody();
        if (rb != null)
        {
            rb.AddForce(mainCam.transform.forward * dropForce + Vector3.up * dropUpwardForce, ForceMode.Impulse);
        }
        currentItem = null;
    }

    public Gr_BaseItemInteractable GetCurrentItem()
    {
        return currentItem;
    }
    public void UseCurrentItem()
    {
        if (currentItem is IUsableItem usableItem)
        {
            if (usableItem.CanUse)
            {
                usableItem.Use();
            }
        }
    }
}
