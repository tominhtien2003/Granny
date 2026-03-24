using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCoinGainScript : MonoBehaviour
{
    public double m_coinPerPunch;
    public double m_baseCoinPerPunch;
    public float m_currentMultiplier;
    public MoneyReceiveAnimator m_coinMoney;

    public void SetPunchCoin(double damage)
    {
        m_baseCoinPerPunch = damage * .1f;
        SetRealCoinGain();

    }

    public void Multiplier(float multiplier)
    {
        m_currentMultiplier = multiplier;
        SetRealCoinGain();
    }

    void SetRealCoinGain()
    {
        m_coinPerPunch = m_baseCoinPerPunch * m_currentMultiplier;
    }

    public void AddCoin()
    {
        m_coinMoney.StartAnimate(m_coinPerPunch);
    }
}
