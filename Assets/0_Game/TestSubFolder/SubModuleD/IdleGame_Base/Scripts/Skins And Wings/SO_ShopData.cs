using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public enum ShopItemType
{
    None = 0,
    Ads = 1,
    IAP = 2,
}
[System.Serializable]
public class SkinShopData : ShopItem
{
    public bool m_wingAvailable;
    public Mesh m_mesh;
    public List<Material> m_materials;
    public Sprite m_raceIcon;
}

[CreateAssetMenu(fileName = "Shop_Data", menuName = "Data/Shop_Data")]
public class SO_ShopData : ScriptableObject
{
    [Header("SKINS")]
    public List<SkinShopData> m_shopItemData = new();
#if UNITY_EDITOR
    public List<GameObject> m_initData;
    
    [ContextMenu("InitAdd")]
    public void InitAdd()
    {
        foreach (var go in m_initData)
        {
            if (go == null) continue;

            // Try to get MeshFilter and Renderer
            var meshFilter = go.GetComponentInChildren<SkinnedMeshRenderer>();

            if (meshFilter != null)
            {
                SkinShopData data = new SkinShopData();
                data.m_mesh = meshFilter.sharedMesh;
                data.m_itemName = data.m_mesh.name;
                data.m_materials = new List<Material>();
                foreach (var mat in meshFilter.sharedMaterials)
                {
                    data.m_materials.Add(mat);
                }

                m_shopItemData.Add(data);
            }
            else
            {
                Debug.LogWarning($"Prefab '{go.name}' has no MeshFilter/Renderer.");
            }
        }
    }


    public void InitDuplicate()
    {
        foreach (var v in m_shopItemData)
        {
            v.m_itemAdAmount = (m_shopItemData.IndexOf(v) - 1) / 8 + 1;
            v.m_raceIcon = v.m_displayIcon;
        }
        this.SO_SetDirty();
        AssetDatabase.SaveAssets(); 
    }
    public List<Sprite> avatars;
    [ContextMenu("SetSprite")]
    public void SetSprite()
    {
        for (int i = 0; i < m_shopItemData.Count; i++)
        {
            m_shopItemData[i].m_displayIcon = avatars[i];
        }
        this.SO_SetDirty();
        AssetDatabase.SaveAssets(); 
    }    
#endif

}