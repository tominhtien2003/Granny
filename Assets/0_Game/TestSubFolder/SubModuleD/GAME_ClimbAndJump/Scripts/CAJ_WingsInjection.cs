using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAJ_WingsInjection : MonoBehaviour
{
    public SimpleTrussClimb m_climb;
    private void Awake()
    {
        GetComponent<WingsContainer>().m_stats = m_climb;
    }
}
