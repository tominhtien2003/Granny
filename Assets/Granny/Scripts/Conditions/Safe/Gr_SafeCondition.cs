using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Gr_SafeCondition : Gr_BaseConditionInteractable
{
    [SerializeField] Vector3 direct;

    private Vector3 start, end;
    private bool isOpened = false;
    private bool isMoving = false;

    private void Start()
    {
        start = transform.localEulerAngles;
        end = start + direct * 180;
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
        Tween.LocalEulerAngles(transform, start, end, .5f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
        });
    }

    private void CloseObj()
    {
        Tween.LocalEulerAngles(transform, end, start, .5f, Ease.Linear).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
        });
    }
}
