using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLine : MonoBehaviour
{
    public LineRenderer m_line;
    public Transform m_start;
    public Transform m_end;
    public bool m_fixedY;
    
    void Update()
    {
        Vector3 pos1 = m_start.position;
        Vector3 pos2 = m_end.position;

        pos1.y += .05f;
        pos2.y += 0.5f;
        m_line.SetPosition(0, pos1);
        m_line.SetPosition(1, pos2);
    }
    public void SetDes(Transform st, Transform ed, bool fixedY = false)
    {
        m_start = st;
        m_end = ed;
        m_fixedY = fixedY;
    }
}
