using System;
using UnityEngine;

public class Gr_PlayerInteractor : MonoBehaviour
{
    [SerializeField] private LayerMask interacLayer;
    [SerializeField] private float maxdistance = 6f;

    private Camera mainCam;
    private Gr_IInteractable current;
    private Collider lastHitCollider;

    public Action<Gr_IInteractable> OnObjectInteractChanged;

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
        Ray ray = mainCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxdistance, interacLayer))
        {
            if (hit.collider != lastHitCollider)
            {
                lastHitCollider = hit.collider;
                Gr_IInteractable newInteractable = hit.collider.GetComponentInParent<Gr_IInteractable>();

                if (newInteractable != current)
                {
                    ChangeInteractable(newInteractable);
                }
            }
        }
        else
        {
            if (current != null)
            {
                lastHitCollider = null;
                ChangeInteractable(null);
            }
        }
    }

    private void ChangeInteractable(Gr_IInteractable newInteractable)
    {
        current?.OnFocusExit();
        current = newInteractable;
        current?.OnFocusEnter();

        OnObjectInteractChanged?.Invoke(current);
    }

    public void Interact()
    {
        current?.Interact();
    }
}