using UnityEngine;
namespace BloxLikeBasic
{
    public class StateMachineInitializer : MonoBehaviour
    {
        public BloxStateController StateController;

        public NormalMovement m_normalMovement;
        public TrussClimbing m_trussClimbing;
        public DeathState m_deathState;

        protected BloxStateBlackboard m_blackboard;
        protected bool m_canClimb = false;
        protected bool m_isDead = false;

        private void Awake()
        {
            m_blackboard = StateController.m_blackboard;

            m_normalMovement.OnWallHitEvent += CheckTruss;
            m_normalMovement.OnWallHitEvent += CheckKillblock;
            m_deathState.OnDeathStateComplete += () => m_isDead = false;

            StateController.AddTransition(m_normalMovement, m_deathState, new FuncPredicate(() => m_isDead));
            StateController.AddTransition(m_deathState, m_normalMovement, new FuncPredicate(() => !m_isDead));
            StateController.AddTransition(m_normalMovement, m_trussClimbing, new FuncPredicate(CanClimb));
            StateController.AddTransition(m_trussClimbing, m_normalMovement, new FuncPredicate(() => m_trussClimbing.CheckExit()));

        }


        protected virtual void CheckKillblock(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint)
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
}
