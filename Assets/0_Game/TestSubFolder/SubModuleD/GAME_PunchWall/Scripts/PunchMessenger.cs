using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchMessenger : MonoBehaviour
{
    public PunchWallScript m_punchWall;
    public void PunchImpact()
    {
        m_punchWall.PunchCurrentWall();
    }
}
