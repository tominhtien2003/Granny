using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAJ_FallSoundAddon : MonoBehaviour
{
    public ClimbFall m_fallState;
    private void Awake()
    {
        m_fallState.OnStateEnterEvent += (() => AudioManager.Instance.SetWindSound(true));
        m_fallState.OnGroundHitEvent += (() =>
        {
            AudioManager.Instance.PlayImpactSound();
            AudioManager.Instance.SetWindSound(false);
        });
    }
}
