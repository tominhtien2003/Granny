using KinematicCharacterController;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_Zipline : MonoBehaviour
{
    [SerializeField] float duration;
    bool isUsed;
    [SerializeField] G3_Switch switchObj;
    [SerializeField] Transform destination, player, playerParent, playerLastParent;
    [SerializeField] Animator playerAnim;
    Vector3 initPos;
    Coroutine autoDownRoutine;
    private void Start()
    {
        initPos = transform.localPosition;
        playerLastParent = player.parent;
    }
    public void Active()
    {
        player.SetParent(playerParent, false);
        player.GetComponent<KinematicCharacterMotor>().enabled = false;
        player.position = playerParent.transform.position;
        player.rotation = playerParent.transform.rotation;
        playerAnim.Play("Swing");
        Tween.Position(transform, destination.position, duration,
            Ease.Linear, 1, CycleMode.Yoyo, 1f).OnComplete(() => OnReachDestination());
        if (!isUsed)
        {
            isUsed = true;
        }
    }
    void OnReachDestination()
    {
        var motor = player.GetComponent<KinematicCharacterMotor>();

        Vector3 worldPos = player.position;
        Quaternion worldRot = player.rotation;

        player.SetParent(playerLastParent, false);
        player.position = worldPos;
        player.rotation = worldRot;

        motor.SetPosition(worldPos);
        motor.SetRotation(worldRot);
        motor.ForceUnground();

        motor.enabled = true;
        StartAutoDown();
    }


    public void Back()
    {
        transform.localPosition = initPos;
        isUsed = false;
        switchObj.ResetSwitch();
    }

    void StartAutoDown()
    {
        if (autoDownRoutine != null)
            StopCoroutine(autoDownRoutine);

        autoDownRoutine = StartCoroutine(AutoDownAfterDelay());
    }

    IEnumerator AutoDownAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        Back();
    }
}
