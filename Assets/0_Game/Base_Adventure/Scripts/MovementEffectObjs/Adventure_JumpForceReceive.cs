using BloxLikeBasic;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_JumpForceReceive : MonoBehaviour
{
    [SerializeField] KinematicCharacterMotor motor;
    [SerializeField] NormalMovement movement;
    [SerializeField] BloxBrain brain;
    [SerializeField] float baseAirSpeed, baseJumpupSpeed;
    Coroutine jumpRoutine;
    void Awake()
    {
        baseAirSpeed = movement.MaxAirMoveSpeed;
        baseJumpupSpeed = movement.JumpUpSpeed;
    }
    public void JumpForce(float multiple)
    {
        if (jumpRoutine != null)
        {
            StopCoroutine(jumpRoutine);
            ResetAirSpeed();
        }

        jumpRoutine = StartCoroutine(IEJumpForce(multiple));
    }
    void ResetAirSpeed()
    {
        movement.MaxAirMoveSpeed = baseAirSpeed;
        movement.JumpUpSpeed = baseJumpupSpeed;
    }
    IEnumerator IEJumpForce(float multiple)
    {
        movement.MaxAirMoveSpeed = baseAirSpeed * multiple;
        movement.JumpUpSpeed = baseJumpupSpeed * multiple;

        motor.ForceUnground();

        Vector3 velocity = motor.Velocity;
        velocity -= Vector3.Project(velocity, motor.CharacterUp);

        velocity += motor.CharacterUp * movement.JumpUpSpeed;

        motor.BaseVelocity = velocity;

        yield return new WaitUntil(() => !motor.GroundingStatus.FoundAnyGround);
        yield return new WaitUntil(() => motor.GroundingStatus.FoundAnyGround);

        ResetAirSpeed();
        jumpRoutine = null;
    }

    public Vector3 GetVelocity(Vector3 direction)
    {
        return transform.TransformDirection(direction) * movement.JumpUpSpeed;
    }
}
