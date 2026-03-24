using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_HeightMoneyClimbAddon : MonoBehaviour
{
    [SerializeField] private SlideClimb m_climbState;
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
        float height = m_climbState.m_blackboard.Motor.TransientPosition.y;
        if (m_currentHeight == height) return;
        m_currentHeight = height;
        m_heightMoneyController.m_currentHeight = m_currentHeight;
        m_heightMoneyController.CalculatePotentialMoney();

    }
    private void Update()
    {
        if (!m_isClimbing) return;
        SetCurrentHeight();
    }
}
