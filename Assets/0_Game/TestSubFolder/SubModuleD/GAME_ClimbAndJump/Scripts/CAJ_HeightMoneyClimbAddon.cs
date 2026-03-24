using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAJ_HeightMoneyClimbAddon : MonoBehaviour
{
    [SerializeField] private SimpleTrussClimb m_climbState;
    public float m_currentHeight;
    private bool m_isClimbing = false;
    private HeightMoneyController m_heightMoneyController;
    private void Awake()
    {
        m_heightMoneyController = GetComponent<HeightMoneyController>();
        m_climbState.OnStateChanged += OnClimbStateSwitched;
    }
    public void OnClimbStateSwitched(bool isClimbing)
    {
        m_isClimbing = isClimbing;
        SetCurrentHeight();
    }
    void SetCurrentHeight()
    {
        if (m_currentHeight == m_climbState.m_blackboard.Motor.TransientPosition.y) return;
        m_currentHeight = m_climbState.m_blackboard.Motor.TransientPosition.y;
        m_heightMoneyController.m_currentHeight = m_currentHeight;
        m_heightMoneyController.CalculatePotentialMoney();
        
    }
    private void Update()
    {
        if (!m_isClimbing) return;
        SetCurrentHeight();
    }
}
