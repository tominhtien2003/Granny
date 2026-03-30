using PrimeTween;
using UnityEngine;

public class Gr_BarricadePlank : Gr_BaseOpenableObject
{
    [SerializeField] private Gr_BaseOpenableObject obj;
    private const string interactableLayer = "Interactable";
    private const string defaultLayer = "Default";
    protected override void Start()
    {
        base.Start();
        start = transform.localEulerAngles;
        end = start + direct * 100f;
    }
    public override void CloseObj()
    {
        Tween.LocalEulerAngles(transform, end, start, .2f, Ease.OutQuad).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
            obj.SetInteractableState(interactableLayer);
        });
    }

    public override void OpenObj()
    {
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.OutQuad).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
            obj.SetInteractableState(defaultLayer);
        });
    }

    
}
