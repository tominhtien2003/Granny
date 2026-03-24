using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_WingsInjection: MonoBehaviour
{
    public SlideClimb m_climb;
    private void Awake()
    {
        GetComponent<WingsContainer>().m_stats = m_climb;
    }
}
