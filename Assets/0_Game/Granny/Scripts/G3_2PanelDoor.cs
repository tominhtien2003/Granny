using PrimeTween;
using PrimeTweenDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_2PanelDoor : G3_Door
{
    [SerializeField] Transform door2;
    [SerializeField] Vector3 rotationAxis = Vector3.up;
    [Button]
    public override void OpenDoor()
    {
        Vector3 axis = rotationAxis.normalized;
        Tween.LocalRotation(door, openDir * 90 * axis, .5f, Ease.Linear);
        Tween.LocalRotation(door2, -openDir * 90 * axis, .5f, Ease.Linear);
        G3_AudioManager.Instance.PlayDoorSound();
        _triggerBox.gameObject.SetActive(false);
        isOpened = true;
        ToggleOutline(false);
        if (waypointWait != null)
        {
            waypointWait.CanMove = true;
        }
    }
}
