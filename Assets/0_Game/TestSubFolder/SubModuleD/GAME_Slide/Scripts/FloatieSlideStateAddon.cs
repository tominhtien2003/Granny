using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatieSlideStateAddon : MonoBehaviour
{
    public SlideDown m_slideState;
    private FloatieSwapper m_floatieSwapper;
    public GameObject m_particle;
    public PlayerWingDataSetter m_dataSetter;
    private bool m_cachedState;
    void Awake()
    {
        m_dataSetter = FindObjectOfType<PlayerWingDataSetter>();
        m_floatieSwapper = GetComponent<FloatieSwapper>();
        m_slideState.OnStateChanged += SetFloatiePos;
        m_dataSetter.m_onWingAssigned += () => SetFloatiePos(m_cachedState);
    }

    void SetFloatiePos(bool state)
    {
        m_cachedState = state;
        m_floatieSwapper.SetFloatiePos(state);
        m_particle.SetActive(state);
        
    }
}
