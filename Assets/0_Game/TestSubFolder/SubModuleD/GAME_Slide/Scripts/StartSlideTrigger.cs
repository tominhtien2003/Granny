using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSlideTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SlideStateMachineInitializer slideStateMachine = other.GetComponentInChildren<SlideStateMachineInitializer>();
            if (slideStateMachine != null)
            {
                slideStateMachine.StartSlide = true;
            }
        }
    }
}
