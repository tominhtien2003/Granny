using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ClimbFallReceiveAddon : MonoBehaviour
{
    public ClimbAndJumpStateMachineInitializer m_stateIniter;
    private ReceiveMoneyScript m_moneyReceive;
    private void Awake()
    {
        m_moneyReceive = GetComponent<ReceiveMoneyScript>();
        m_stateIniter.OnGroundHitEvent += FallEvent;
    }
    void FallEvent()
    {
        m_moneyReceive.FallEvent();
    }
    
}
