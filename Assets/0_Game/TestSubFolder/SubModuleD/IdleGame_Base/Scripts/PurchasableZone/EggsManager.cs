using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct MeshInfo
{
    public Mesh m_mesh;
    public Material m_mat;
}
public class EggsManager : MonoBehaviour
{
    public EggPurchasableZone[] m_eggs;
    public EggsData m_currentMapEggData;
    public HatchEggPopup m_hatchPopup;
    //public Floor_HatchEggPopup floorHatchPopup;
    public List<MeshInfo> m_eggMMList = new List<MeshInfo>();
    private void Awake()
    {
        m_eggs = GetComponentsInChildren<EggPurchasableZone>(true);
        for (int i = 0; i < m_eggs.Length; i++)
        {
            m_eggs[i].m_petData = i;
            m_eggs[i].OnUnlockDone += OpenPopup;
        }
    }
    public void SetEggPos(List<Transform> pos)
    {
        for (int i =0; i < m_eggs.Length; i++)
        {
            m_eggs[i].transform.position = pos[i].position;
        }
    }
    void OpenPopup(int i)
    {
        if (m_hatchPopup)
        {
            m_hatchPopup.SetCurrentPetPool(i, m_eggMMList[i]);
            m_hatchPopup.Show();
        }/*else if (floorHatchPopup)
        {
            floorHatchPopup.SetCurrentPetPool(i, m_eggMMList[i]);
            floorHatchPopup.Show();
        }*/
        
    }
    public void SetEggsData(EggsData data)
    {
        if (m_currentMapEggData == data) return;
        m_currentMapEggData = data;
        if (m_hatchPopup)
        {
            m_hatchPopup.m_currentData = data;
            m_hatchPopup.m_currentEggStart = data.m_currentEggStart;
        }/*else if (floorHatchPopup)
        {
            floorHatchPopup.m_currentData = data;
            floorHatchPopup.m_currentEggStart = data.m_currentEggStart;
        }*/

        for (int i = 0; i < m_eggs.Length; i++)
        {
            m_eggs[i].SetPrice(data.m_eggs[i].m_price);
        }
    }
}
