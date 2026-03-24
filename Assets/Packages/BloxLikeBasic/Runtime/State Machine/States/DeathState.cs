using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathState : BaseMovementBloxState
{
    [SerializeField] private float m_respawnTime;
    private float m_currentRespawnTime;

    public UnityAction OnDeathStateComplete;
    public override void AfterCharacterUpdate(float deltaTime)
    {
        
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }

    public override void OnStateExit()
    {
        Motor.BaseVelocity = Vector3.zero;
        Motor.enabled = true;
        Motor.Capsule.enabled = true;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Motor.BaseVelocity = Vector3.zero;
        Motor.enabled = false;
        Motor.Capsule.enabled = false;
        m_currentRespawnTime = 0;
    }
    public override void OnStateUpdate(float dt)
    {
        m_currentRespawnTime += dt;
        if (m_currentRespawnTime >= m_respawnTime) OnDeathStateComplete?.Invoke();
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        
    }

}
