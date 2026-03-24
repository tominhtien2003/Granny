using KinematicCharacterController;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseMovementBloxState : MonoBehaviour, IBloxState, ICharacterController
{
    public BloxStateBlackboard m_blackboard;
    public RuntimeAnimatorController m_stateAnimationController;
    public List<Collider> IgnoredColliders = new List<Collider>();

    protected KinematicCharacterMotor Motor;
    protected BloxInputHolder InputHolder;

    public UnityAction<Collider, Vector3, Vector3> OnWallHitEvent;
    public void SetBlackboard(BloxStateBlackboard blackboard)
    {
        m_blackboard = blackboard;
        Motor = m_blackboard.Motor;
        InputHolder = m_blackboard.InputHolder;
    }
    public abstract void AfterCharacterUpdate(float deltaTime);
    public abstract void BeforeCharacterUpdate(float deltaTime);
    public virtual bool IsColliderValidForCollisions(Collider coll)
    {
        if (IgnoredColliders.Count == 0)
        {
            return true;
        }

        if (IgnoredColliders.Contains(coll))
        {
            return false;
        }

        return true;
    }
    public virtual void OnDiscreteCollisionDetected(Collider hitCollider) { }
    public virtual void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
        OnWallHitEvent?.Invoke(hitCollider, hitNormal, hitPoint);
    }
    public virtual void OnStateEnter()
    {
        if (m_stateAnimationController != null && m_blackboard.Animator.runtimeAnimatorController != m_stateAnimationController) m_blackboard.Animator.runtimeAnimatorController = m_stateAnimationController;
        Motor.CharacterController = this;
    }

    public abstract void OnStateExit();


    public abstract void OnStateUpdate(float dt);
    public virtual void PostGroundingUpdate(float deltaTime) { }
    public virtual void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
    public abstract void UpdateRotation(ref Quaternion currentRotation, float deltaTime);
    public abstract void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);

}
