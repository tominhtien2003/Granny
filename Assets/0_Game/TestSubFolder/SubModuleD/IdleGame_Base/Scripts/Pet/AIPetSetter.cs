using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPetSetter : MonoBehaviour
{
    public AIPetRotation[] m_ais;
    public List<PetItem> m_assigns;
    private SO_PetItemDataV2 m_data;
    private int m_cnt;
    public void StartInit()
    {
        m_data = ClimbAndJump_DataController.Instance.m_petData;
        m_cnt = m_data.m_petItems.Length;
        m_ais = GetComponentsInChildren<AIPetRotation>();
        
    }
    public void AssignAIPets()
    {
        for (int i = 0; i < m_ais.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                m_assigns[j] = m_data.m_petItems[Random.Range(0, m_cnt)];
            }
            m_ais[i].SetPetSkins(m_assigns);
        }
    }
}
