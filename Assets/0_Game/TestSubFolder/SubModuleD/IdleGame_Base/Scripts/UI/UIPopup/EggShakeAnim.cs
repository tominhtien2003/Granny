using LitMotion;
using LitMotion.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggShakeAnim : MonoBehaviour
{
    public Ease m_easeType;
    public float m_endValue;
    public float m_duration;
    public float m_delay;
    public int m_frequency;

    private void Start()
    {
        LMotion.Punch.Create(Vector3.zero, Vector3.right * m_endValue, m_duration)
            .WithEase(m_easeType)
            .WithLoops(-1, LoopType.Restart)
            .WithDelay(m_delay, DelayType.EveryLoop).WithFrequency(10).BindToLocalEulerAngles(transform);
    }
}
