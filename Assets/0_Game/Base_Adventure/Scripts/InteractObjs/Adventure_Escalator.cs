using PrimeTween;
using System.Collections;
using UnityEngine;

public class Adventure_Escalator : MonoBehaviour
{
    [SerializeField] float duration;
    bool isUsed, isUnlockedForBot = false, playerOn;
    [SerializeField] GameObject limit;
    [SerializeField] Adventure_EscalatorSwitch escalatorSwitch;
    Coroutine autoDownRoutine;
    [SerializeField] float zPositionInit, zPositionTarget;
    [SerializeField] WaypointScript waypointWaitOn,waypointWaitBefore;
    private void Start()
    {
        zPositionInit = transform.localPosition.z;
    }
    public void GoUp(bool byPlayer)
    { 
        waypointWaitOn.CanMove = false;
        if(limit != null)
        {
            limit.SetActive(true);
        }
        Tween.LocalPositionZ(transform, zPositionTarget, duration,
            Ease.Linear, 1, CycleMode.Yoyo, 1f).OnComplete(()=> OnReachTop());
        if (!isUsed)
        {
            isUsed = true;
        }
        playerOn = byPlayer;
        G3_AudioManager.Instance.PlayEscalatorSound();
    }
    void OnReachTop()
    {
        if (limit != null)
        {
            limit.SetActive(false);
        }
        isUnlockedForBot = true;

        if( /*(player.parent != transform*/
            playerOn == false)
        {
            StartAutoDown();
        }
        waypointWaitBefore.CanMove = true;
    }


    public void GoDown()
    {
        waypointWaitBefore.CanMove = false;
        Tween.LocalPositionZ(transform, zPositionInit, duration, Ease.Linear, 1
        ).OnComplete(() =>
        {
            isUsed = false;
            escalatorSwitch.ResetSwitch();
            waypointWaitOn.CanMove = true;
        });
    }

    void StartAutoDown()
    {
        if (autoDownRoutine != null)
            StopCoroutine(autoDownRoutine);

        autoDownRoutine = StartCoroutine(AutoDownAfterDelay());
    }

    IEnumerator AutoDownAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        GoDown();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isUsed) return;

        if (other.CompareTag("G3_Bot") && isUnlockedForBot)
        {
            GoUp(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        //player.SetParent(playerLastParent);
        playerOn = false;
        StartAutoDown();
    }
    public void SetForBot()
    {
        isUnlockedForBot = true;
    }
}
