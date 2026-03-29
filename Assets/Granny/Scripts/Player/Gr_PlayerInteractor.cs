using System;
using UnityEngine;

public class Gr_PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float maxdistance = 6f;
    [SerializeField] private LayerMask mask;

    private Camera mainCam;
    private Gr_IInteractable current;
    private Collider lastHitCollider;
    private int interactableLayerIndex;

    private void Awake()
    {
        interactableLayerIndex = LayerMask.NameToLayer("Interactable");
    }
    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        FindObjectCanInteract();
    }

    private void FindObjectCanInteract()
    {
        var ray = mainCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));

        if (Physics.Raycast(ray, out var hit, maxdistance, mask))
        {
            if (hit.collider.gameObject.layer == interactableLayerIndex)
            {
                if (hit.collider == lastHitCollider) return;
                lastHitCollider = hit.collider;
                var newInteractable = hit.collider.GetComponentInParent<Gr_IInteractable>();

                if (newInteractable != current)
                {
                    ChangeInteractable(newInteractable);
                }
            }
            else
            {
                ClearInteractable();
            }
        }
        else
        {
            ClearInteractable();
        }
    }
    private void ClearInteractable()
    {
        if (current == null) return;

        lastHitCollider = null;
        ChangeInteractable(null);
    }
    private void ChangeInteractable(Gr_IInteractable newInteractable)
    {
        current?.OnFocusExit();
        current = newInteractable;
        current?.OnFocusEnter();

        Gr_EventManager.Notify(new ObjectInteractableChangedEvent(current));
    }
}