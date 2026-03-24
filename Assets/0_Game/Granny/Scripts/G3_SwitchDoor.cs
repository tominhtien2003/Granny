using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_SwitchDoor : MonoBehaviour, IActiveBySwitch
{
    [SerializeField] float posZTarget;
    [SerializeField] protected WaypointScript waypointWait;
    public void OpenDoor()
    {
        Tween.LocalPositionZ(transform, posZTarget, .5f, Ease.Linear);
        if (waypointWait != null)
        {
            waypointWait.CanMove = true;
        }
    }
    public void Active()
    {
        OpenDoor();
    }
}
