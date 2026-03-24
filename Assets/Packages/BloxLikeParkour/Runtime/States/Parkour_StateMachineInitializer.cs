using BloxLikeBasic;
using Qu.BloxLike;
using UnityEngine;
public class Parkour_StateMachineInitializer : MonoBehaviour
{
    public BloxStateController StateController;

    public NormalMovement m_normalMovement;
    public TrussClimbing m_trussClimbing;
    public Parkour_DeathState m_deathState;

    private BloxStateBlackboard m_blackboard;
    public bool IsDead = false;

    public DeathToggler m_deathToggler;
    public CheckTrussToggler m_trussToggler;

    private void Awake()
    {
        m_blackboard = StateController.m_blackboard;
        m_trussToggler = new CheckTrussToggler(m_blackboard.Motor, m_blackboard.m_trussLayer, m_trussClimbing);
        m_normalMovement.OnWallHitEvent += m_trussToggler.CheckTruss;

        StateController.AddTransition(m_normalMovement, m_deathState, new FuncPredicate(() => m_deathToggler.IsDead));
        StateController.AddTransition(m_deathState, m_normalMovement, new FuncPredicate(() => !m_deathToggler.IsDead));
        StateController.AddTransition(m_normalMovement, m_trussClimbing, new FuncPredicate(()=>m_trussToggler.CanClimb));
        StateController.AddTransition(m_trussClimbing, m_normalMovement, new FuncPredicate(m_trussToggler.CheckFinishClimb));

    }


    
}
