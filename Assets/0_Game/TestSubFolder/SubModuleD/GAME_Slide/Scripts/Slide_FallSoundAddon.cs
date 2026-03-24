using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide_FallSoundAddon : MonoBehaviour
{
    public SlideDown m_fallState;
    private void Awake()
    {
        m_fallState.OnStateChanged += CheckState;
    
    }
    void CheckState(bool b)
    {
        if (b)
        {
            AudioManager.Instance.SetWindSound(true);
        }
        else
        {
            AudioManager.Instance.PlayImpactSound();
            AudioManager.Instance.SetWindSound(false);
        }
    }
}
