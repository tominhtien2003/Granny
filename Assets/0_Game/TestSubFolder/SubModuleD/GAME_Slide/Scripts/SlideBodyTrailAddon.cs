using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBodyTrailAddon : MonoBehaviour
{
    public TrailRenderer[] m_trailRenderers;
    public SlideDown SlideDown;
    [Button]
    public void Setup()
    {
        m_trailRenderers = GetComponentsInChildren<TrailRenderer>();
        SlideDown = GetComponentInChildren<SlideDown>();

    }

    public void Awake()
    {
        SlideDown.OnStateChanged += SetTrails;
    }
    void SetTrails(bool state)
    {
        foreach (var trail in m_trailRenderers)
        {
            trail.emitting = state;
        }
    }
}
