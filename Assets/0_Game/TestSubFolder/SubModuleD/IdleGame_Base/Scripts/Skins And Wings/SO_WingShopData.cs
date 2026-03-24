using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[Serializable]
public class WingDisplayData: ShopItem
{
    public SO_WingsData m_wingsData;
    public float m_climbSpeed;
    public int m_gemPrice;
}
[CreateAssetMenu(fileName = "Wing_Shop_Data", menuName = "Data/Wing_Shop_Data")]
public class SO_WingShopData : ScriptableObject
{
    public List<WingDisplayData> m_wingShopItemData = new();
    public float m_startingSpeed;
#if UNITY_EDITOR
    public List<SO_WingsData> m_wingDatas;

    //public List<GameObject> m_initWings;
    public GameObject m_initOne;

    public void InitSpeed()
    {
        m_wingShopItemData[0].m_climbSpeed = m_startingSpeed;
        for (int i = 1; i < m_wingShopItemData.Count; i++)
        {
            m_wingShopItemData[i].m_climbSpeed = m_wingShopItemData[i - 1].m_climbSpeed * 1.4f;
            m_wingShopItemData[i].m_itemAdAmount = 1;
        }
        for (int i = 6; i < m_wingShopItemData.Count; i++)
        {
            m_wingShopItemData[i].m_itemPrice = m_wingShopItemData[i - 1].m_itemPrice * 4.5f;
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }
    public void InitFromWingsData()
    {
        m_wingShopItemData.Clear();
        for(int i= 0; i < m_wingDatas.Count; i++)
        {
            var d = m_wingDatas[i];
            var k = new WingDisplayData();
            k.m_wingsData = d;
            k.m_itemName = d.name;
            if (d.m_materials == null) d.m_materials = new List<Material>();
            m_wingShopItemData.Add(k);
        }
    }
    /*public void InitGloves()
    {
        for (int i= 0; i < m_initWings.Count; i++)
        {
            var w = m_initWings[i];
            var d = m_wingDatas[i];
            var k = new WingDisplayData();
            k.m_wingsData = d;
            k.m_itemName = w.name;
            d.name = w.name;
            if (d.m_materials == null) d.m_materials = new List<Material>();
            d.m_materials.Clear();
            foreach (var mat in w.GetComponentInChildren<MeshRenderer>().sharedMaterials)
            {
                d.m_materials.Add(mat);
            }
            d.m_renderer = w.GetComponentInChildren<MeshFilter>().sharedMesh;
           
            m_wingShopItemData.Add(k);
            UnityEditor.EditorUtility.SetDirty(d);
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }*/

    public void InitName()
    {
        for (int i = 0; i < m_wingShopItemData.Count; i++)
        {
            m_wingShopItemData[i].m_itemName = (i + 1).ToString();
        }
    }
    public void AddInitFloaties()
    {
        foreach (var w in m_wingDatas)
        {
            var k = new WingDisplayData();
            k.m_wingsData = w;
            k.m_displayIcon = m_wingShopItemData[m_wingShopItemData.Count - 1].m_displayIcon;
            m_wingShopItemData.Add(k);
        }
    }
    public void InitGemPrices()
    {
        for (int i = 0; i < m_wingShopItemData.Count; i++)
        {
            var v = m_wingShopItemData[i];
            v.m_itemPrice = 0;
            v.m_itemAdAmount = i + 1;
            v.m_gemPrice = v.m_itemAdAmount * 10;

        }
        this.SO_SetDirty();
        AssetDatabase.SaveAssets(); 
    }
    public void AddInitBoats()
    {
        MeshFilter[] f = m_initOne.GetComponentsInChildren<MeshFilter>();
        for (int i= 0; i < f.Length; i++)
        {
            var k = new WingDisplayData();
            var d = ScriptableObject.CreateInstance<SO_WingsData>();
            d.name = "Boat_" + (i + 1).ToString();
            d.m_mesh = f[i].sharedMesh;
            d.m_materials = new List<Material>();
            foreach (var mat in f[i].GetComponent<MeshRenderer>().sharedMaterials)
            {
                d.m_materials.Add(mat);
            }
            k.m_wingsData = d;
            k.m_itemName = d.name;
            //k.m_displayIcon = m_wingShopItemData[m_wingShopItemData.Count - 1].m_displayIcon;
            m_wingShopItemData.Add(k);
            AssetDatabase.CreateAsset(d, "Assets/0_Game/IdleGame/GAME_KAYAK_SLIDE/Data/Boats_Data/" + d.name + ".asset");
            UnityEditor.EditorUtility.SetDirty(d);
        }
    }
    public void FixInitBoats()
    {
        MeshFilter[] f = m_initOne.GetComponentsInChildren<MeshFilter>();
        for (int i= 0; i < m_wingShopItemData.Count; i++)
        {
            var k = m_wingShopItemData[i];
            var d = k.m_wingsData;
            d.m_mesh = f[i].sharedMesh;
            d.m_materials = new List<Material>();
            foreach (var mat in f[i].GetComponent<MeshRenderer>().sharedMaterials)
            {
                d.m_materials.Add(mat);
            }
            UnityEditor.EditorUtility.SetDirty(d);
        }
    }

    [ContextMenu("ResetMoney")]
    void ResetMoney()
    {
        
        for (int i= 0; i < m_wingShopItemData.Count; i++)
        {
            m_wingShopItemData[i].m_itemPrice = 0;
            m_wingShopItemData[i].m_itemAdAmount = Mathf.Clamp((i - 5) / 2, 1, 4);
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

}
