using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Gr_Door : Gr_BaseOpenableObject
{
    protected override void Start()
    {
        base.Start();
        start = transform.localEulerAngles;
        end = start + direct * 90;
    }

    public override void OpenObj()
    {
        col.enabled = false;
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
            col.enabled = true;
        });
    }

    public override void CloseObj()
    {
        col.enabled = false; 
        Tween.LocalEulerAngles(transform, end, start, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
            col.enabled = true;
        });
    }
}
