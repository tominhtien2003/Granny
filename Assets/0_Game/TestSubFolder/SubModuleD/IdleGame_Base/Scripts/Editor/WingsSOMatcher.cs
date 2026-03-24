using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WingsSOMatcher : EditorWindow
{
    public SO_WingShopData m_data;

    public DefaultAsset m_spritesFolder;
    public DefaultAsset m_materialsFolder;
    public DefaultAsset m_wingSOFolder;

    [MenuItem("Tools/Assign Wings From Folder")]
    static void Init()
    {
        GetWindow<WingsSOMatcher>("Wings Assigner");
    }

    void OnGUI()
    {
        GUILayout.Label("Assign Sprites Folder", EditorStyles.boldLabel);

        m_spritesFolder = (DefaultAsset)EditorGUILayout.ObjectField("Sprites Folder", m_spritesFolder, typeof(DefaultAsset), false);
        m_materialsFolder = (DefaultAsset)EditorGUILayout.ObjectField("Materials Folder", m_materialsFolder, typeof(DefaultAsset), false);
        m_wingSOFolder = (DefaultAsset)EditorGUILayout.ObjectField("Wings Folder", m_wingSOFolder, typeof(DefaultAsset), false);
        m_data = (SO_WingShopData)EditorGUILayout.ObjectField("Wing Data", m_data, typeof(SO_WingShopData), false);

        if (GUILayout.Button("Assign Wings"))
        {
            if (m_spritesFolder == null 
                || m_materialsFolder == null
                || m_wingSOFolder == null
                || m_data == null)
            {
                Debug.LogError("Please assign folders.");
                return;
            }
            AssignWings();
        }
        if (GUILayout.Button("Assign Sprites To Wings"))
        {
            if (m_spritesFolder == null
                || m_data == null)
            {
                Debug.LogError("Please assign folders.");
                return;
            }
            AssignSpritesToWings();
        }
    }
    void AssignSpritesToWings()
    {
        string wingsPath = AssetDatabase.GetAssetPath(m_spritesFolder);
        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { wingsPath });
    
        var spriteDict = new Dictionary<string, Sprite>();

        foreach (string guid in spriteGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null && !spriteDict.ContainsKey(sprite.name))
                spriteDict.Add(sprite.name, sprite);
        }

        for (int i = 0; i < m_data.m_wingShopItemData.Count; i++)
        {
           
            var item = m_data.m_wingShopItemData[i];
            string it = m_data.m_wingShopItemData[i].m_itemName;
            if (!spriteDict.TryGetValue(it, out Sprite foundSprite))  Debug.Log("Cant find sprite with the name " + it);
            else item.m_displayIcon = foundSprite;
        }

        Undo.RecordObject(m_data, "Assign wings sprites");
        EditorUtility.SetDirty(m_data);
        AssetDatabase.SaveAssets();
        Debug.Log("Complete");

    }
    void AssignWings()
    {
        string wingsPath = AssetDatabase.GetAssetPath(m_spritesFolder);

        // Load sprites from folder
        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { wingsPath });
        string[] meshGuids = AssetDatabase.FindAssets("t:Mesh", new[] { wingsPath });
        var spriteDict = new Dictionary<string, Sprite>();
        var meshDict = new Dictionary<string, Mesh>();


        foreach (string guid in spriteGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null && !spriteDict.ContainsKey(sprite.name))
                spriteDict.Add(sprite.name, sprite);
        }
        foreach (string guid in meshGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            string n = mesh.name.Substring(2);
            if (mesh != null && !meshDict.ContainsKey(n))
                meshDict.Add(n, mesh);
        }


        // Load mats from folder
        string matsPath = AssetDatabase.GetAssetPath(m_materialsFolder);
        string[] mats = AssetDatabase.FindAssets("t:Material", new[] { matsPath });
        var matsDict = new Dictionary<string, Material>();

        foreach (string guid in mats)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null && !matsDict.ContainsKey(mat.name))
                matsDict.Add(mat.name, mat);
        }

        // Load wingSOData from folder
        string soPath = AssetDatabase.GetAssetPath(m_wingSOFolder);
        string[] wings = AssetDatabase.FindAssets("t:SO_WingsData", new[] { soPath });
        var soDict = new Dictionary<string, SO_WingsData>();

        foreach (string guid in wings)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SO_WingsData w = AssetDatabase.LoadAssetAtPath<SO_WingsData>(path);
            
            if (w != null && !soDict.ContainsKey(w.name))
                soDict.Add(w.name, w);
            if (matsDict.ContainsKey(w.name)) w.m_materials[0] = matsDict[w.name];
            if (meshDict.ContainsKey(w.name)) w.m_mesh = meshDict[w.name];
            Undo.RecordObject(w, "Assign mesh and mats");
            EditorUtility.SetDirty(w);
        }

        /*for (int i = 0; i < m_data.m_wingShopItemData.Count; i++)
        {
            string it = (i + 1).ToString();
            var item = m_data.m_wingShopItemData[i];
            if (!spriteDict.TryGetValue(it, out Sprite foundSprite))  Debug.Log("Cant find sprite with the name " + it);
            else item.m_displayIcon = foundSprite;
            if (!soDict.TryGetValue(it, out SO_WingsData foundWing))  Debug.Log("Cant find wings with the name " + it);
            else item.m_wingsData = foundWing;

        }
        Undo.RecordObject(m_data, "Assign wing datas");
        EditorUtility.SetDirty(m_data);*/
        AssetDatabase.SaveAssets();
        Debug.Log("Complete");
    }
}
