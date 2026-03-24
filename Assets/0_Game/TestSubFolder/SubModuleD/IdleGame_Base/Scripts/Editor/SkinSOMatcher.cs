using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkinSOMatcher : EditorWindow
{
    public SO_ShopData m_data;

    public DefaultAsset m_spritesFolder;

    [MenuItem("Tools/Assign Skins From Folder")]
    static void Init()
    {
        GetWindow<SkinSOMatcher>("Skins Assigner");
    }

    void OnGUI()
    {
        GUILayout.Label("Assign Sprites Folder", EditorStyles.boldLabel);

        m_spritesFolder = (DefaultAsset)EditorGUILayout.ObjectField("Sprites and Meshes Folder", m_spritesFolder, typeof(DefaultAsset), false);
        
        m_data = (SO_ShopData)EditorGUILayout.ObjectField("Data", m_data, typeof(SO_ShopData), false);

        if (GUILayout.Button("Assign"))
        {
            if (m_spritesFolder == null 
                || m_data == null)
            {
                Debug.LogError("Please assign folders.");
                return;
            }
            AssignFloaties();
        }
    }
    void AssignFloaties()
    {
        string wingsPath = AssetDatabase.GetAssetPath(m_spritesFolder);
        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { wingsPath });
    
        var spriteDict = new Dictionary<string, Sprite>();

        foreach (string guid in spriteGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            Debug.Log(sprite.name);
            if (sprite != null && !spriteDict.ContainsKey(sprite.name))
                spriteDict.Add(sprite.name, sprite);
        }

        for (int i = 15; i < m_data.m_shopItemData.Count; i++)
        {
           
            var item = m_data.m_shopItemData[i];
            //if (item.m_raceIcon != null) continue;
            if (!spriteDict.TryGetValue(item.m_itemName + ".png", out Sprite foundSprite))  Debug.Log("Cant find sprite with the name " + item.m_itemName);
            else item.m_raceIcon = foundSprite;
        }

        Undo.RecordObject(m_data, "Assign floaties datas");
        EditorUtility.SetDirty(m_data);
        AssetDatabase.SaveAssets();
        Debug.Log("Complete");

    }
    
}
