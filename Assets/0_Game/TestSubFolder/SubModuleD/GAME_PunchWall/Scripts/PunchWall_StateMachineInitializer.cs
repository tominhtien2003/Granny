using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PunchWall_StateMachineInitializer : MonoBehaviour
{
    public BloxStateController StateController;

    public NormalMovement m_normalMovement;
    public PunchWall_NormalMovement m_punchWallMovement;
    public TrainingState m_trainState;
    public CannonShotState m_cannonState;
    public ClimbFall m_fallState;

    private BloxStateBlackboard m_blackboard;
    public bool IsTraining;
    private AnimatorOverrideController m_overrideAnim;
    public PunchWallInteractSwapper m_swapper;
    public PunchWallScript m_trainingScript;

    public UnityAction OnGroundHitEvent;
    private void Awake()
    {
        m_blackboard = StateController.m_blackboard;
        m_overrideAnim = new AnimatorOverrideController(m_normalMovement.m_stateAnimationController);
        m_normalMovement.m_stateAnimationController = m_overrideAnim;
        m_trainState.m_stateAnimationController = m_overrideAnim;
        m_cannonState.m_stateAnimationController = m_overrideAnim;

        StateController.AddTransition(m_normalMovement, m_punchWallMovement, new FuncPredicate(() => m_punchWallMovement.AutoPunch));
        StateController.AddTransition(m_punchWallMovement, m_normalMovement, new FuncPredicate(() => !m_punchWallMovement.AutoPunch));

        StateController.AddTransition(m_normalMovement, m_trainState, new FuncPredicate(() => IsTraining));
        StateController.AddTransition(m_trainState, m_normalMovement, new FuncPredicate(() => !IsTraining));
       
        StateController.AddTransition(m_normalMovement, m_cannonState, new FuncPredicate(() => m_cannonState.CannonIn));
        StateController.AddTransition(m_cannonState, m_fallState, new FuncPredicate(() => !m_cannonState.CannonIn));

        StateController.AddTransition(m_fallState, m_normalMovement, new FuncPredicate(ClimbFallToNormalMovement));

        m_cannonState.OnStateChanged += (bool b) => m_trainingScript.CanPunchExternal = false;
        m_fallState.OnStateChanged += (bool b) => { if (!b)
            {
                m_trainingScript.CanPunchExternal = true;
                m_trainingScript.SetReturnState(false);
            }
        };
    }

    public void SetAutoPunchState(bool state)
    {
        m_punchWallMovement.AutoPunch = state;
    }
    bool ClimbFallToNormalMovement()
    {
        if (m_fallState.HitGround)
        {
            OnGroundHitEvent?.Invoke();
            return true;
        }
        return false;
    }
    public void SetCannon()
    {
        m_cannonState.CannonIn = true;
    }
    private TrainingMachine m_currentMachine;
    public void SetTraining(Transform anchor, AnimationClip animIdle, AnimationClip animMove, Animator m_machineAnim, TrainingMachine machine)
    {
        m_currentMachine = machine;
        m_currentMachine.ChangeOccupiedState(true);
        IsTraining = true;
       

        m_overrideAnim["Squatting_Idle"] = animIdle;
        m_overrideAnim["Squatting"] = animMove;
        m_trainState.SetAnchor(anchor);
        m_trainState.m_trainingMachineAnim = m_machineAnim;
        m_swapper.SetTrainState(true);
        
    }
    public void LeaveTraining()
    {
        IsTraining = false;
        m_swapper.SetTrainState(false);
        PunchWall_DataController.SaveBodypartStrength();
        
        m_currentMachine.ChangeOccupiedState(false);
        m_currentMachine = null;
    }
   
}
