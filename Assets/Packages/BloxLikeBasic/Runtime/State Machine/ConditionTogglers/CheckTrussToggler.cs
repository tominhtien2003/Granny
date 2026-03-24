using Cinemachine.Utility;
using KinematicCharacterController;
using UnityEngine;
namespace Qu.BloxLike
{
    public class CheckTrussToggler
    {
        private KinematicCharacterMotor Motor;
        private LayerMask m_trussLayer;
        private TrussClimbing m_climbState;
        public bool CanClimb = false;
        public CheckTrussToggler(KinematicCharacterMotor motor, LayerMask trussLayer, TrussClimbing climbState)
        {
            Motor = motor;
            m_trussLayer = trussLayer;
            m_climbState = climbState;
        }
        public bool CheckFinishClimb()
        {
            bool b=m_climbState.CheckExit();
            CanClimb = !b;
            return b;
        }
        public void CheckTruss(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint)
        {
            if (hitNormal.y > 0.5f) return;
            hitNormal.y = 0;
            Vector3 velo = Motor.BaseVelocity;
            velo.y = 0;
            if (((1 << hitCollider.gameObject.layer) & m_trussLayer) > 0
                && velo.sqrMagnitude > .1f
                && Vector3.Angle(-hitNormal, Motor.CharacterForward) < 30f
                && Vector3.Angle(-hitNormal, velo) < 30f)
                {
                Debug.DrawLine(hitPoint, hitPoint - hitNormal * 100f, Color.red, 10f);
                Debug.DrawRay(Motor.TransientPosition, velo.normalized * 100f, Color.magenta, 10f);
                Debug.Log("Hit Truss " + Motor.BaseVelocity, hitCollider);
                    m_climbState.CurrentTruss = hitCollider as BoxCollider;
                    CanClimb = true;
                    return;
                }
            
        }
    }
}
