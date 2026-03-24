using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrailAddon : MonoBehaviour
{
    public TrailRenderer[] m_trailRenderers;
    public ClimbFall m_climbFallScript;
    [Button]
    public void Setup()
    {
        m_trailRenderers = GetComponentsInChildren<TrailRenderer>();
        m_climbFallScript = GetComponentInChildren<ClimbFall>();
        
    }

    public void Awake()
    {
        m_climbFallScript.OnStateEnterEvent += () => SetTrails(true);
        m_climbFallScript.OnGroundHitEvent += () => SetTrails(false);
    }
    void SetTrails(bool state)
    {
        foreach (var trail in m_trailRenderers)
        {
            trail.emitting = state;
        }
    }
}
