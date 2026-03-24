using BloxLikeBasic;
using Cinemachine.Utility;
using KinematicCharacterController;
using UnityEngine;

public class FootstepSoundsAddon : MonoBehaviour
{
    public KinematicCharacterMotor m_motor;
    public NormalMovement m_movement;
   
    private bool m_currentState;
    private void Start()
    {     
        m_movement.OnJumpPerformed += AudioManager.Instance.PlayJumpSound;
    }
    void Update()
    {
        CheckState();
    }

    void CheckState()
    {
        Vector3 v = m_motor.BaseVelocity;
        v.y = 0;
        bool b = v.sqrMagnitude > .001f && m_motor.GroundingStatus.IsStableOnGround;
        if (b == m_currentState) return;
        m_currentState = b;
        AudioManager.Instance.SetFootStepSound(b);
    }

    public void ForceStopSound()
    {
        m_currentState = false;
        AudioManager.Instance.SetFootStepSound(m_currentState);
    }
}
