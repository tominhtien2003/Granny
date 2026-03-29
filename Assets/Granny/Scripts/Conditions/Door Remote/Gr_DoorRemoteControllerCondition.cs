using PrimeTween;
using UnityEngine;

public class Gr_DoorRemoteControllerCondition : Gr_BaseConditionInteractable
{
    [SerializeField] private Collider colDoor;
    [SerializeField] Vector3 direct;

    private Vector3 start, end;
    private bool isOpened = false;
    private bool isMoving = false;

    private void Start()
    {
        start = transform.localEulerAngles;
        end = start + direct * 90;
    }

    protected override void ResolveConditions()
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
    private void OpenObj()
    {
        colDoor.enabled = false;
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
            colDoor.enabled = true;
        });
    }

    private void CloseObj()
    {
        colDoor.enabled = false; 
        Tween.LocalEulerAngles(transform, end, start, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
            colDoor.enabled = true;
        });
    }
}
