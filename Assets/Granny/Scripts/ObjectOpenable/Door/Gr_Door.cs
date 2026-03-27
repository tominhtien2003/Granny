using PrimeTween;
using UnityEngine;

public class Gr_Door : Gr_BaseOpenableObject
{

    [SerializeField] private Collider colDoor;

    private void Start()
    {
        start = transform.localEulerAngles;
        end = start + direct * 90;
    }

    public override void OpenObj()
    {
        colDoor.enabled = false;
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
            colDoor.enabled = true;
        });
    }

    public override void CloseObj()
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
