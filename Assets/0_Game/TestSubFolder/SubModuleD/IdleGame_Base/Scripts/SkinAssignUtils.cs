using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkinAssignUtils
{
    public static void AssignModel(SkinnedMeshRenderer mesher, SkinShopData shopData)
    {
        if (mesher == null) return;
        if (shopData.m_mesh != null) mesher.sharedMesh = shopData.m_mesh;
        if (shopData.m_materials != null)
        {
            Material[] sharedMaterials = new Material[shopData.m_materials.Count];

            for (int i = 0; i < shopData.m_materials.Count; i++)
            {
                sharedMaterials[i] = shopData.m_materials[i];
            }
            mesher.sharedMaterials = sharedMaterials;

        }

    }
    public static void AssignModel(MeshFilter meshFilter, MeshRenderer meshRenderer, WingSkinShopData shopData)
    {
        if (meshFilter == null) return;
        if (meshRenderer == null) return;
        if (shopData.m_mesh != null) meshFilter.sharedMesh = shopData.m_mesh;
        if (shopData.m_materials != null)
        {
            Material[] sharedMaterials = new Material[shopData.m_materials.Count];

            for (int i = 0; i < shopData.m_materials.Count; i++)
            {
                sharedMaterials[i] = shopData.m_materials[i];
            }
            meshRenderer.sharedMaterials = sharedMaterials;
        }
    }
    public static void AssignModel(MeshFilter meshFilter, Mesh mesh)
    {
        if (meshFilter == null) return;
        if (mesh != null) meshFilter.sharedMesh = mesh;
    }
    public static void AssignModel(MeshRenderer meshRenderer, List<Material> materials)
    {
        if (meshRenderer == null) return;
        if (materials != null)
        {
            Material[] sharedMaterials = new Material[materials.Count];

            for (int i = 0; i < materials.Count; i++)
            {
                sharedMaterials[i] = materials[i];
            }
            meshRenderer.sharedMaterials = sharedMaterials;
        }
    }
}
