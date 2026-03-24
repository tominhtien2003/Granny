using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrussGraphicModifier : MonoBehaviour
{
    public float m_offset = 14.654f;
    public float m_offsetX = 14.654f;
#if UNITY_EDITOR

    public List<GameObject> m_trussMeshes;

    public List<Material> m_mats;
    [Button]
    public void SetColors()
    {
        if (m_mats.Count == 0) return;
        
        LODGroup[] mr = GetComponentsInChildren<LODGroup>();
        MeshRenderer[] m;
        int per = mr.Length / m_mats.Count;
        int curr = 0;
        for (int i = 0; i < mr.Length; i++)
        {
            m = mr[i].GetComponentsInChildren<MeshRenderer>();
            foreach (var mm in m)mm.sharedMaterial = m_mats[curr];
            if (i >= per)
            {
                curr++;
                per = per * (curr + 1);
            }
        }
    }
    [Button]
    public void SetTrusses()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(m_offsetX * i, m_offset * i, 0);
        }
        
    }
    [Button]
    public void SetTrussesMeshes()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = m_trussMeshes[i % m_trussMeshes.Count].GetComponent<MeshFilter>().sharedMesh;
        }
        
    }
#endif
}
