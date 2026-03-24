using PrimeTween;
using UnityEngine;

public class Adventure_EscalatorSwitch : Adventure_InteractObj
{
    [SerializeField] Adventure_Escalator escalator;
    //[SerializeField] Transform switchTransform;
    //[SerializeField] Vector3 originRotation = new Vector3(0, 75, 0);
    public override void Interact()
    {
        base.Interact();
        escalator.GoUp(true);
        _triggerBox.gameObject.SetActive(false);
        //Tween.LocalRotation(transform, Vector3.zero, .5f,
        //     Ease.Linear, 1);
    }
    public void ResetSwitch()
    {
        _triggerBox.gameObject.SetActive(true);
        //Tween.LocalRotation(transform, originRotation, .5f,
        //     Ease.Linear, 1);
    }
}
