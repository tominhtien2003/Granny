using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToSpaceStateMachineInitializer : MonoBehaviour
{
    public BloxStateController StateController;

    public NormalMovement m_normalMovement;
    public ClimbFall m_climbFall;
    public SpaceJump m_spaceJump;
    public SpaceRun m_spaceRun;
    public SpaceStand m_spaceStand;

    private BloxStateBlackboard m_blackboard;

    public float m_fallSpeedThreshold = -10f;
    public bool m_canSpaceJump = false;
    private void Awake()
    {
        m_blackboard = StateController.m_blackboard;

       

        StateController.AddTransition(m_normalMovement, m_spaceRun, new FuncPredicate(CanSpaceJump));
        StateController.AddTransition(m_spaceRun, m_spaceJump, new FuncPredicate(CanSpaceJump));

        StateController.AddTransition(m_spaceJump, m_climbFall, new FuncPredicate(() => m_blackboard.Motor.Velocity.y < m_fallSpeedThreshold - 1f));
        StateController.AddTransition(m_climbFall, m_spaceStand, new FuncPredicate(() => m_climbFall.HitGround));
        StateController.AddTransition(m_spaceStand, m_normalMovement, new FuncPredicate(()=>m_spaceStand.CanMove));
    }
    public void SetDirection(Vector3 forward)
    {
        m_spaceRun.SetRunDirection(forward);
    }
    bool CanSpaceJump()
    {
        if (m_canSpaceJump)
        {
            this.LogError("space jump!");
            m_canSpaceJump = false;
            return true;
        }
        return false;
    }
}
