using System.Collections;

#if  UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

public class DEV_SKIN_ADDON : MonoBehaviour
{
    public GameObject m_pet;
    public SO_ShopData m_petData;
    public int m_currentIndex;

    public string m_wrapName;
    public string m_folderPath;
    public DEV_RENDER_TEXTURE_EXPORTER m_screenshot;

    private SkinnedMeshRenderer m_render;
    [Button]
    public void GetRendererAndFilter()
    {
        m_render = m_pet.GetComponent<SkinnedMeshRenderer>();
    }
    public void IterateList(int dir)
    {
        m_currentIndex += dir;
        if (m_currentIndex < 0) m_currentIndex += m_petData.m_shopItemData.Count;
        m_currentIndex %= m_petData.m_shopItemData.Count;
        SetPet(m_currentIndex);
    }
    void SetPet(int index)
    {
        Material[] sharedMaterials = new Material[m_petData.m_shopItemData[index].m_materials.Count];

        for (int i = 0; i < m_petData.m_shopItemData[index].m_materials.Count; i++)
        {
            sharedMaterials[i] = m_petData.m_shopItemData[index].m_materials[i];
        }
        m_render.sharedMaterials = sharedMaterials;
       
        m_render.sharedMesh = m_petData.m_shopItemData[index].m_mesh;

        //m_screenshot._fileName = m_folderPath + m_petData.m_shopItemData[index].m_itemName + ".png";
    }
    [Button]
    public void GoLeft()
    {
        IterateList(-1);
    }
    [Button]
    public void GoRight()
    {
        IterateList(1);
    }
}

  #endif