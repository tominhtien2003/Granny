using PrimeTween;
using UnityEngine;

public class Gr_ManholeCover : Gr_BaseOpenableObject
{
    [SerializeField] private float offset = 1f;
    protected override void Start()
    {
        base.Start();
        start = transform.localPosition;
        end = start + direct * offset;
    }
    public override void CloseObj()
    {
        Tween.LocalPosition(transform, end, start, .2f, Ease.OutQuad).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
        });
    }

    public override void OpenObj()
    {
        Tween.LocalPosition(transform, start, end, .2f, Ease.OutQuad).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
        }); ;
    }
}
