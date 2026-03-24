using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteSOMatcher : EditorWindow
{
    public SO_PetItemDataV2 m_petData;

    public DefaultAsset spritesFolder;  

    [MenuItem("Tools/Assign Sprites From Folder")]
    static void Init()
    {
        GetWindow<SpriteSOMatcher>("Sprite Assigner");
    }

    void OnGUI()
    {
        GUILayout.Label("Assign Sprites Folder", EditorStyles.boldLabel);

        spritesFolder = (DefaultAsset)EditorGUILayout.ObjectField("Sprites Folder", spritesFolder, typeof(DefaultAsset), false);
        m_petData = (SO_PetItemDataV2)EditorGUILayout.ObjectField("Pet Data", m_petData, typeof(SO_PetItemDataV2), false);

        if (GUILayout.Button("Assign Sprites"))
        {
            if (spritesFolder == null || m_petData == null)
            {
                Debug.LogError("Please assign both folders.");
                return;
            }
            AssignSprites();
        }
    }

    void AssignSprites()
    {
        string spritePath = AssetDatabase.GetAssetPath(spritesFolder);

        // Load sprites from folder
        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { spritePath });
        var spriteDict = new Dictionary<string, Sprite>();

        foreach (string guid in spriteGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null && !spriteDict.ContainsKey(sprite.name))
                spriteDict.Add(sprite.name, sprite);
        }

        foreach (var item in m_petData.m_petItems)
        {
            if (item != null && spriteDict.TryGetValue(item.m_itemName, out Sprite foundSprite))
            {
                Undo.RecordObject(m_petData, "Assign Sprite");
                Debug.Log("found " + item.m_itemName);
                item.m_displayIcon = foundSprite; // Replace with your sprite field
                EditorUtility.SetDirty(m_petData);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Complete");
    }
}
