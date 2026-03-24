using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.CullingGroup;

public class TrainingState : BaseMovementBloxState, IStateChangeEvents
{
    public Animator m_trainingMachineAnim;
    public override void AfterCharacterUpdate(float deltaTime)
    {
        
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }
    public void SetAnchor(Transform anchor)
    {
        Motor.SetPositionAndRotation(anchor.position, anchor.rotation);
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        OnStateChanged?.Invoke(true);
        //TODO:Play anim
        m_blackboard.Animator.CrossFadeInFixedTime("TrainIdle", .1f);
        m_trainingMachineAnim.CrossFadeInFixedTime("Idle", .1f);
        Motor.BaseVelocity = Vector3.zero;
    }

    public override void OnStateExit()
    {
        m_blackboard.Animator.CrossFadeInFixedTime("StableGrounded", .1f);
        OnStateChanged?.Invoke(false);
        SetAnimSpeed(1);
    }
    public int CurrentMultiplier = 1;

    public UnityAction<bool> OnStateChanged {get; set; }

    void OnTap()
    {

    }
    public void SetLiftAnim()
    {
        //play lift anim every time screen is pressed
        m_blackboard.Animator.CrossFadeInFixedTime("TrainMove", 0.1f);
        m_trainingMachineAnim.CrossFadeInFixedTime("Move", .1f);

    }
    public void SetAnimSpeed(float speed)
    {
        m_blackboard.Animator.SetFloat("TrainingSpeed", speed);
        m_trainingMachineAnim.SetFloat("TrainingSpeed", speed);
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
