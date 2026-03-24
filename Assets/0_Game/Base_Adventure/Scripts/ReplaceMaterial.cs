using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ReplaceMaterials : MonoBehaviour
{
    public string materialFolder = "Assets/0_Game/3DModel/Games/G3_GrumpyGran/Material";

    private Dictionary<string, Material> materialCache;

    [ContextMenu("Replace Materials")]
    public void ReplaceAll()
    {
#if UNITY_EDITOR
        LoadMaterials();

        Renderer[] renderers = FindObjectsOfType<Renderer>();
        int replaceCount = 0;

        foreach (var rend in renderers)
        {
            var mats = rend.sharedMaterials;
            bool changed = false;

            for (int i = 0; i < mats.Length; i++)
            {
                var oldMat = mats[i];
                if (oldMat == null) continue;

                string matName = oldMat.name;

                if (materialCache.TryGetValue(matName, out Material newMat))
                {
                    mats[i] = newMat;
                    changed = true;
                    replaceCount++;
                }
            }

            if (changed)
                rend.sharedMaterials = mats;
        }

        Debug.Log("Changed " + replaceCount + " material.");
#endif
    }

#if UNITY_EDITOR
    void LoadMaterials()
    {
        materialCache = new Dictionary<string, Material>();

        string[] files = Directory.GetFiles(materialFolder, "*.mat", SearchOption.AllDirectories);
        foreach (string filePath in files)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(filePath.Replace("\\", "/"));
            if (mat != null)
            {
                if (!materialCache.ContainsKey(mat.name))
                    materialCache.Add(mat.name, mat);
            }
        }

        Debug.Log("Loaded " + materialCache.Count + " materials from folder.");
    }
#endif
}
