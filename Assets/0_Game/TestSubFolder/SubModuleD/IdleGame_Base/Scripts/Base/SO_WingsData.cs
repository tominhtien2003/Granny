using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "Wings_Data", menuName = "Data/Wings_Data")]
public class SO_WingsData : ScriptableObject
{
    public WingSkinShopData m_wingSkinShopData;
    public Mesh m_mesh;
    public List<Material> m_materials;
    public GameObject m_particle;
    public Vector3 m_overrideScale;
    public Vector3 m_overrideRot;
    public Vector3 m_overridePos;

    public bool m_overrideSkin = false;
    public Vector3 m_skinOverridePos;

#if UNITY_EDITOR
    [ContextMenu("SetSprite")]
    void SetAgain()
    {
        m_wingSkinShopData = new WingSkinShopData();
        m_wingSkinShopData.m_mesh = m_mesh;
        m_wingSkinShopData.m_materials = m_materials;
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

}
[System.Serializable]
public class WingSkinShopData : ShopItem
{
    public Mesh m_mesh;
    public List<Material> m_materials;
}
