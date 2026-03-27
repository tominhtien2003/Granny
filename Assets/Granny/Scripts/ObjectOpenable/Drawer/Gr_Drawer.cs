using PrimeTween;
using UnityEngine;

public class Gr_Drawer : Gr_BaseOpenableObject
{
    [SerializeField] private float offset = 2f;
    private void Start()
    {
        start = transform.localPosition;
        end = start + offset * direct;
    }
    public override void CloseObj()
    {
        Tween.LocalPosition(transform, end, start, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
        });
    }

    public override void OpenObj()
    {
        Tween.LocalPosition(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
        });
    }
}
