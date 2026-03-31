using UnityEngine;

public abstract class Gr_InteractableBase : MonoBehaviour, Gr_IInteractable
{
    public virtual void Interact()
    {

    }

    public virtual void OnFocusEnter()
    {
    }

    public virtual void OnFocusExit()
    {
    }
    
    public Transform GetTransform()
    {
        return transform;
    }
}