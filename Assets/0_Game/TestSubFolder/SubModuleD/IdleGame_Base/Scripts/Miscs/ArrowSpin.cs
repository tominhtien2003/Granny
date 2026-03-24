using LitMotion;
using LitMotion.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpin : MonoBehaviour
{
    public List<RectTransform> m_tfs;
    public RectTransform m_thisTransform;
    private MotionHandle m_arrowHandle;
    public int m_currentIndx;
    public void StartHit()
    {
        this.enabled = true;
        float tallest = -10000;
        for (int i = 0; i< m_tfs.Count; i++)
        {
            float g = m_tfs[i].position.x;
            if (g < m_thisTransform.position.x) continue;
            if (m_tfs[i].position.y > tallest)
            {
                tallest = m_tfs[i].position.y;
                m_currentIndx = i;
            }
        }
    }
    public void EndHit()
    {
        this.enabled = false;
        m_arrowHandle.TryCancel();
    }
    void Update()
    {
        if (m_tfs[m_currentIndx].position.x < m_thisTransform.position.x)
        {
            m_currentIndx = (m_currentIndx + 1) % m_tfs.Count;
            m_arrowHandle.TryCancel();
            m_arrowHandle = LMotion.Create(152.84f, 180f, .2f).BindToLocalEulerAnglesZ(transform);
        }
    }
    private void OnDestroy()
    {
        m_arrowHandle.TryCancel();
    }
}
