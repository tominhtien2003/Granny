using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMoneyController : MoneyController
{
    [SerializeField] private SpaceJump m_spaceJumpState;
    [SerializeField] private double m_heightPerMoney = 10f;
    private bool m_isFlying = false;

    private void Start()
    {
        m_spaceJumpState.OnStateChanged += OnSpaceJumpStateSwitched;
    }
    void OnSpaceJumpStateSwitched(bool isFlying)
    {
        m_isFlying = isFlying;
        if (isFlying)
        {
            m_cachedY = -1;
            m_cachedBaseMoney = 0;
        }
    }
    private float m_cachedY;
    private double m_cachedBaseMoney;
    protected override void CalculateBaseMoney(ref double baseMoney)
    {
        if (m_spaceJumpState.transform.position.y < m_cachedY)
        {
            baseMoney = m_cachedBaseMoney;
            return;
        }
        baseMoney = m_spaceJumpState.transform.position.y / m_heightPerMoney;
        m_cachedY = m_spaceJumpState.transform.position.y;
        m_cachedBaseMoney = baseMoney;

    }

    private void FixedUpdate()
    {
        if (!m_isFlying) return;
        CalculatePotentialMoney();
    }
}
