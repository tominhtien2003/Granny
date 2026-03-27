using PrimeTween;
using UnityEngine;

public class Gr_Lever : Gr_BaseOpenableObject
{
    [SerializeField] private float angle = 60f;
    [SerializeField] private Gr_SecretDoor secretObj;

    private void Start()
    {
        start = transform.localEulerAngles;
        end = start + direct * angle;
    }
    public override void CloseObj()
    {
        Tween.LocalEulerAngles(transform, end, start, .2f, Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
            isOpened = false;
            InteractSecretObj();
        });
    }

    public override void OpenObj()
    {
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
            isOpened = true;
            InteractSecretObj();
        });
    }
    private void InteractSecretObj()
    {
        if (isOpened)
        {
            secretObj.OpenObj();
        }
        else
        {
            secretObj.CloseObj();
        }
    }
}
