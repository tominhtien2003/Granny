using System.Collections;
using UnityEngine;

public class Adventure_ElevatorController : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] Adventure_ElevatorDoor door;
    [SerializeField] Adventure_ElevatorPlatform platform;
    [SerializeField] Adventure_ElevatorTrigger trigger;
    [SerializeField] Adventure_ElevatorSwitch switcher;

    [Header("Position")]
    [SerializeField] float zStart;
    [SerializeField] float zEnd;

    bool waitingInside;
    bool waitingExit;
    bool moving;
    bool botUnlocked = false; 

    [SerializeField] WaypointScript waypointWaitOn, waypointWaitBefore;

    void Awake()
    {
        trigger.Init(this);
        zStart = transform.localPosition.z;
    }

    #region SWITCH CALL
    public void CallFromSwitch()
    {
        if (moving || waitingInside || waitingExit)
            return;
        StartCoroutine(OpenAtBottom());
    }

    IEnumerator OpenAtBottom()
    {
        yield return door.Open();
        waitingInside = true;
    }
    #endregion

    #region TRIGGER CALLBACK
    public void OnEnter(PlayerType type)
    {
        if (type == PlayerType.Bot && botUnlocked)
        {
            if (!waitingInside && !moving && !waitingExit)
            {
                StartCoroutine(OpenAtBottom());
                return;
            }
        }

        if (!waitingInside)
            return;

        waitingInside = false;
        StartCoroutine(CloseAndGoUp());
    }

    public void OnExit()
    {
        if (!waitingExit)
            return;
        waitingExit = false;
        StartCoroutine(CloseAndReturn());
    }
    #endregion

    #region BOT UNLOCK
    public void UnlockForBot()
    {
        botUnlocked = true;
        Debug.Log("Elevator unlocked for bot!");
    }

    public void LockForBot()
    {
        botUnlocked = false;
    }
    #endregion

    #region FLOW
    IEnumerator CloseAndGoUp()
    {
        yield return door.Close();
        waypointWaitOn.CanMove = false;
        moving = true;
        platform.MoveToZ(zEnd)
            .OnComplete(() => StartCoroutine(OpenAtTop()));
    }

    IEnumerator OpenAtTop()
    {
        G3_AudioManager.Instance.PlayElevatorReachSound();
        moving = false;
        yield return door.Open();
        waitingExit = true;
        waypointWaitBefore.CanMove = true;
    }

    IEnumerator CloseAndReturn()
    {
        yield return door.Close();
        moving = true;
        waypointWaitBefore.CanMove = false;
        platform.MoveToZ(zStart)
            .OnComplete(() =>
            {
                waypointWaitOn.CanMove = true;
                switcher.ResetSwitch();
                moving = false;
            });
    }
    #endregion
}