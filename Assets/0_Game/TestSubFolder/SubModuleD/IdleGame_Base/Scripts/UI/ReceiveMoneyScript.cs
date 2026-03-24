using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveMoneyScript : MonoBehaviour
{
    public MoneyReceiveAnimator m_animator;
    public PlayerGroundCrackSetter m_crackSetter;
    public void FallEvent()
    {
        if (m_animator.m_controller.m_currentPotentialMoney < 1) return;
        m_crackSetter.EnableCrack();
        m_animator.m_beforeAmount = m_animator.m_controller.m_totalMoney;
        m_animator.m_controller.AddPotentialMoney(false);
        m_animator.m_afterAmount = m_animator.m_controller.m_totalMoney;
        m_animator.AnimateMoneyReceive();
        m_animator.m_controller.ResetPotentialMoney();
    }
}
