using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Parkour_DeathState : BaseMovementBloxState,IStateChangeEvents
{
    public UnityAction<bool> OnStateChanged { get ; set; }

    public override void AfterCharacterUpdate(float deltaTime)
    {

    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void OnStateExit()
    {
        Motor.enabled = true;
        Motor.BaseVelocity = Vector3.zero;
        Motor.Capsule.enabled = true;
        OnStateChanged?.Invoke(true);
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Motor.enabled = false;
        Motor.BaseVelocity = Vector3.zero;
        Motor.Capsule.enabled = false;
        OnStateChanged?.Invoke(false);
    }
    public override void OnStateUpdate(float dt)
    {
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {

    }

}
