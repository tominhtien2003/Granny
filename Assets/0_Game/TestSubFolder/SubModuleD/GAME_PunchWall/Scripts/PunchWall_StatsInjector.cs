using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWall_StatsInjector : MonoBehaviour
{
    public PunchWallScript m_punchWallScript;
    private void Awake()
    {
        GetComponent<WingsContainer>().m_stats = m_punchWallScript;
    }
}
