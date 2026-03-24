using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTutorialObject : MonoBehaviour
{
    public MoneyController m_coins;
    public IStateChangeEvents m_state;
    public EggPurchasableZone m_firstEgg;
    public GuideLine m_guideLine;

    public Transform m_target1;
    public Transform m_target2;
    private bool m_tutorialOn = false;
    private void Start()
    {
        if (ClimbAndJump_DataController.EggTutorialDone)
        {
            Destroy(gameObject);
            return;
        }

        m_state.OnStateChanged += CheckEnableGuideLine;
        m_firstEgg.OnUnlockDone += OnFirstEggPurchased;
    }

    private void OnFirstEggPurchased(int id)
    {
        if (!m_tutorialOn) return;
        m_guideLine.gameObject.SetActive(false);
        m_firstEgg.OnUnlockDone -= OnFirstEggPurchased;
        ClimbAndJump_DataController.EggTutorialDone = true;
        Destroy(gameObject);
    }

    void CheckEnableGuideLine(bool b)
    {
        if (b) return;
        if (m_coins.m_totalMoney >= 200)
        {
            m_tutorialOn = true;
            m_guideLine.SetDes(m_target1, m_target2);
            m_guideLine.gameObject.SetActive(true);
            m_state.OnStateChanged -= CheckEnableGuideLine;
        }
    }
}
