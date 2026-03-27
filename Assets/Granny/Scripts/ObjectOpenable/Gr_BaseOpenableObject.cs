using UnityEngine;

public abstract class Gr_BaseOpenableObject : Gr_InteractableBase
{
    public Vector3 direct;

    protected Vector3 start, end;
    protected bool isOpened = false;
    protected bool isMoving = false;
    public override void OnFocusEnter()
    {
        
    }
    public override void Interact()
    {
        if (isMoving) return;
        isMoving = true;
        if (isOpened)
        {
            CloseObj();
        }
        else
        {
            OpenObj();
        }
    }
    public override void OnFocusExit()
    {
        
    }
    public abstract void OpenObj();
    public abstract void CloseObj();
}
