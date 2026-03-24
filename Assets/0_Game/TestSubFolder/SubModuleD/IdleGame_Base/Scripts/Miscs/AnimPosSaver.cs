using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPosSaver : MonoBehaviour
{
    public List<Vector3> m_savedPos = new List<Vector3>();
    public List<Quaternion> m_savedRot = new List<Quaternion>();

    [Button]
    public void SavePos()
    {
        m_savedPos.Clear();
        m_savedRot.Clear();
        foreach (var p in transform.GetComponentsInChildren<Transform>())
        {
            m_savedPos.Add(p.localPosition);
            m_savedRot.Add(p.localRotation);
        }
    }

    [Button]
    public void ApplyPos()
    {
        int cnt = 0;
        foreach (var p in transform.GetComponentsInChildren<Transform>())
        {
            
            p.localPosition = m_savedPos[cnt];
            p.localRotation = m_savedRot[cnt];
            cnt++;
        }
    }
}
