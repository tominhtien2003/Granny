using System;
using UnityEngine;
public abstract class SimpleAIScript : MonoBehaviour, ISpeedChanger
{
    protected Vector3 m_moveDir;
    protected Vector3 m_rotDir;
    public float m_moveSpeed;
    public float m_gravity;

    protected bool m_isFalling = false;

    public Animator m_anim;
    protected Vector3 m_pos;


    protected Collider m_selfCollider;
    public SphereVisualizer m_wanderZone;

    public WingsContainer m_wings;
    public float m_climbSpeed = 10f;
    public float m_baseClimbSpeed = 10f;
    public float m_currentGlobalMultiplier = 1f;
    public AIPetRotation m_aiPets;
    protected float m_currentFallSpeed;

    protected virtual void Awake()
    {
        //m_wings = GetComponentInChildren<WingsContainer>();
        m_pos = transform.position;
        if (m_wings != null) m_wings.m_stats = this;
    }
    public abstract void InitBot();
    public void SetSpeed(float multiplier)
    {
        m_climbSpeed = (m_baseClimbSpeed + multiplier) * m_currentGlobalMultiplier;
    }
    public void SetCurrentGlobalMultiplier(float multiplier)
    {
        m_climbSpeed = m_climbSpeed * multiplier / m_currentGlobalMultiplier;
        m_currentGlobalMultiplier = multiplier;
    }

    public void Teleport(Vector3 pos)
    {
        m_pos = pos;
        transform.position = m_pos;
    }
    public abstract void ResetBot();
  
    protected Vector3 m_currentVelocity;
    
    public void SetMoveDir(Vector3 dir, float speed)
    {
        m_moveDir = dir.normalized;
        dir.y = 0;
        m_anim.SetFloat("PlanarSpeed", dir.sqrMagnitude > 0.1f ? m_moveSpeed : 0);
        m_currentVelocity = m_moveDir * speed;

    }

    public virtual void SetModel(SkinShopData shopData)
    {
        
    }
}
