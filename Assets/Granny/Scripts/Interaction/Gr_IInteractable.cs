using UnityEngine;

public interface Gr_IInteractable
{
    void Interact();
    void OnFocusEnter();

    void OnFocusExit();

    Transform GetTransform();
}
