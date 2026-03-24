using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide_AutoInjector : MonoBehaviour
{
    public AutoClimbButton m_button;
    public SlideClimb m_autoState;
    private void Awake()
    {
        m_button.m_autoState = m_autoState;
        m_button.m_state = m_autoState;
    }
}
