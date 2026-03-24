using PrimeTween;
using UnityEngine;

public class G3_Door : Adventure_InteractObj
{
    [SerializeField] protected Transform door;
    [SerializeField] protected bool isOpened = false;
    [SerializeField] protected float openDir = 1;
    [SerializeField] protected WaypointScript waypointWait;
    protected override void Awake()
    {
        base.Awake();
        if (waypointWait != null)
        {
            waypointWait.CanMove = false;
        }
    }
    [Button]
    public virtual void OpenDoor()
    {
        Tween.LocalRotation(door, openDir * 90 * Vector3.forward, .5f, Ease.Linear);
        _triggerBox.gameObject.SetActive(false);
        isOpened = true;
        ToggleOutline(false);
        if(waypointWait!=null)
        {
            waypointWait.CanMove = true;
        }
    }
    public override void Interact()
    {
        if (!_playerInside || G3_Manager.Instance.IsOnCar) return;
        IndicatorController.Instance.HideClickBtn(true);
        ToggleOutline(false);
        if (!isOpened)
        {
            G3_AudioManager.Instance.PlayDoorSound();
            OpenDoor();
        }
    }
}
