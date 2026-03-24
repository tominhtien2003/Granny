using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Pet_Shop_Item_Data3", menuName = "Data/Pet Shop Item Data3")]
public class SO_PetItemDataV2 : ScriptableObject
{
    public PetItem[] m_petItems;
    
    #if UNITY_EDITOR
    
    [Header("EDITOR")]
    public double[] m_multi;

    public void InitPrices()
    {
        for (int i = 0; i < m_petItems.Length; i++)
        {
            m_petItems[i].m_baseMultiplier = m_multi[i];

            m_petItems[i].m_gemPrice = i % 5 * 5;
            m_petItems[i].m_itemAdAmount = 3;

        }
    }
    public void InitMultipliers()
    {
        for (int i = 0; i < m_multi.Length; i ++)
        {
            for (int j = 0; j < 4 && i * 5 + j < m_petItems.Length; j++)
            {
                m_petItems[i * 5 + j].m_baseMultiplier = m_multi[i] * Mathf.Pow(1.5f, j);
            }
            if (i * 5 + 4 < m_petItems.Length) m_petItems[i * 5 + 4].m_baseMultiplier = m_multi[i] * 8;
        }
    }

    public void DEV_FILL_MULTIPLIERS()
    {
        for (int i = 0; i < m_petItems.Length; i++)
        {
            m_petItems[i].m_baseMultiplier = m_multi[i];
            if(i >= m_multi.Length) break;;
        }
        this.SO_SetDirty();
    }
    public void MoveMats()
    {
       /* for (int i = 0; i < m_petItems.Length; i++)
        {
            m_petItems[i].m_mats = new Material[1];

            m_petItems[i].m_mats[0] = m_petItems[i].m_mat;

        }*/
    }
    
    #endif
}
