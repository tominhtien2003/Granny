using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_StateMachineInitializer : MonoBehaviour
{
    public BloxStateController StateController;

    public NormalMovement m_normalMovement;
    public Adventure_FPSMovement m_fpsMovement;
    public Adventure_CarMovement m_carMovement;
    public Jetpack m_jetpack;
    public DeathState m_deathState;
    public TrussClimbing m_trussClimbing;
    public DeathState m_carDeathState;
    private BloxStateBlackboard m_blackboard;
    public DeathToggler m_deathToggler;
    public G3_BotCarTrigger m_botCarTrigger;
    private bool m_canClimb = false;
    private bool m_isDead = false;
    private void Awake()
    {
        m_blackboard = StateController.m_blackboard;
        m_normalMovement.OnWallHitEvent += CheckTruss;
        m_normalMovement.OnWallHitEvent += CheckKillblock;
        m_deathState.OnDeathStateComplete += () => m_isDead = false;
        StateController.AddTransition(m_normalMovement, m_deathState, new FuncPredicate(() => m_deathToggler.IsDead));
        StateController.AddTransition(m_deathState, m_normalMovement, new FuncPredicate(() => !m_deathToggler.IsDead));

        StateController.AddTransition(m_normalMovement, m_trussClimbing, new FuncPredicate(CanClimb));
        StateController.AddTransition(m_trussClimbing, m_normalMovement, new FuncPredicate(() => m_trussClimbing.CheckExit()));
        if (m_carMovement != null)
        {
            if (m_botCarTrigger != null)
            {
                StateController.AddTransition(m_normalMovement, m_carMovement, new FuncPredicate(() => m_botCarTrigger.IsOnCar));
                StateController.AddTransition(m_carMovement, m_normalMovement, new FuncPredicate(() => !m_botCarTrigger.IsOnCar));
            }
            else
            {
                StateController.AddTransition(m_normalMovement, m_carMovement, new FuncPredicate(() => G3_Manager.Instance.IsOnCar));
                StateController.AddTransition(m_carMovement, m_normalMovement, new FuncPredicate(() => !G3_Manager.Instance.IsOnCar));
            }
            
            StateController.AddTransition(m_carMovement, m_carDeathState, new FuncPredicate(() => m_deathToggler.IsDead));
            StateController.AddTransition(m_carDeathState, m_carMovement, new FuncPredicate(() => !m_deathToggler.IsDead));
        }
        if(m_fpsMovement != null)
        {
            StateController.AddTransition(m_normalMovement, m_fpsMovement, new FuncPredicate(() => G3_Manager.Instance.IsInCombat));
            StateController.AddTransition(m_fpsMovement, m_normalMovement, new FuncPredicate(() => !G3_Manager.Instance.IsInCombat));
            StateController.AddTransition(m_fpsMovement, m_deathState, new FuncPredicate(() => m_deathToggler.IsDead));
            StateController.AddTransition(m_deathState, m_fpsMovement, new FuncPredicate(() => !m_deathToggler.IsDead));
        }

        if (m_jetpack != null)
        {
            StateController.AddTransition(m_jetpack, m_deathState, new FuncPredicate(() => m_deathToggler.IsDead));
            StateController.AddTransition(m_normalMovement, m_jetpack, new FuncPredicate(() => m_jetpack.CanEnterJetpack()));
            StateController.AddTransition(m_jetpack, m_normalMovement, new FuncPredicate(() => m_jetpack.CheckExitJetpack()));
            StateController.AddTransition(m_carMovement, m_jetpack, new FuncPredicate(() => m_jetpack.CanEnterJetpack()));
            StateController.AddTransition(m_jetpack, m_carMovement, new FuncPredicate(() => m_jetpack.CheckExitJetpack())); 
            StateController.AddTransition(m_fpsMovement, m_jetpack, new FuncPredicate(() => !G3_Manager.Instance.IsInCombat && m_jetpack.CanEnterJetpack()));
            StateController.AddTransition(m_jetpack, m_fpsMovement, new FuncPredicate(() => G3_Manager.Instance.IsInCombat));
        }
    }

    void CheckKillblock(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint)
    {
        if (m_isDead) return;
        if (hitCollider.gameObject.CompareTag(m_blackboard.m_killTag)) m_isDead = true;
    }
    bool CanClimb()
    {
        if (m_canClimb)
        {
            m_canClimb = false;
            return true;
        }
        return false;
    }
    void CheckTruss(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint)
    {
        if (hitNormal.y > 0.5f) return; // Ignore upward normals
        if (((1 << hitCollider.gameObject.layer) & m_blackboard.m_trussLayer) > 0)
        {

            if (Vector3.Angle(-hitNormal, m_blackboard.Motor.CharacterForward) < 30f)
            {
                Debug.Log("Hit Truss", hitCollider);
                m_trussClimbing.CurrentTruss = hitCollider as BoxCollider;
                m_canClimb = true;
                return;
            }
        }
        m_canClimb = false;
    }
}
