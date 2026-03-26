using PrimeTween;
using UnityEngine;

public class Gr_Door : Gr_InteractableBase
{
    [SerializeField] private Vector3 direct;

    [SerializeField] private Collider colDoor;

    private Vector3 start, end;

    private bool isOpened = false;
    private bool isMoving = false;
    private void Start()
    {
        start = transform.localEulerAngles;
        end = start + direct * 90;
    }
    public override void OnFocusEnter()
    {
        
    }
    public override void Interact()
    {
        if (isMoving) return;
        isMoving = true;
        if (isOpened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
    public override void OnFocusExit()
    {
        
    }
    private void OpenDoor()
    {
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
            colDoor.enabled = true;
        });
    }
    private void CloseDoor()
    {
        Tween.LocalEulerAngles(transform, end, start, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
            colDoor.enabled = true;
        });
    }
}
