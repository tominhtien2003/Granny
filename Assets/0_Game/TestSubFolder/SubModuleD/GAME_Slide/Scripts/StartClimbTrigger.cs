using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StartClimbTrigger : MonoBehaviour
{
    public float m_multi = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SlideStateMachineInitializer slideStateMachine = other.GetComponentInChildren<SlideStateMachineInitializer>();
            if (slideStateMachine != null && Vector3.Angle(Vector3.forward, other.transform.forward) < 120f)
            {
                slideStateMachine.m_climbMultiplier = m_multi;
                slideStateMachine.m_startClimb = transform;
                slideStateMachine.StartClimb = true;
            }
        }
    }
}

