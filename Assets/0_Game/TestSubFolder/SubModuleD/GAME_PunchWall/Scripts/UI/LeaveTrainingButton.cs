using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTrainingButton : MonoBehaviour
{
    public ButtonEffectLogic m_leaveButton;
    public PunchWall_StateMachineInitializer m_initer;
    private void Awake()
    {
        m_leaveButton.onClick.AddListener(m_initer.LeaveTraining);
    }
}
