using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.AI;

public class Gr_DoorRemoteControllerCondition : Gr_BaseConditionInteractable
{
    [SerializeField] private Collider colDoor;
    [SerializeField] Vector3 direct;
    private NavMeshObstacle navMeshObstacle;

    private Vector3 start, end;
    private bool isOpened = false;
    private bool isMoving = false;

    private void Awake()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void Start()
    {
        navMeshObstacle.carving = true;
        start = transform.localEulerAngles;
        end = start + direct * 90;
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
        colDoor.enabled = false;
        Tween.LocalEulerAngles(transform, start, end, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = true;
            isMoving = false;
            colDoor.enabled = true;
            navMeshObstacle.carving = false;
        });
    }

    private void CloseObj()
    {
        navMeshObstacle.carving = true;
        colDoor.enabled = false; 
        Tween.LocalEulerAngles(transform, end, start, .2f, Ease.Linear).OnComplete(() =>
        {
            isOpened = false;
            isMoving = false;
            colDoor.enabled = true;
        });
    }
}
