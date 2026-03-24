#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ColliderAssigner : EditorWindow
{
    //CUBE in Unity has 24 vertices, don't ask why
    private const int VERTEX_THRESHOLD = 24;

    [MenuItem("Tools/Assign Colliders")]
    public static void AssignColliders()
    {
        // Check if any objects are selected
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected. Please select the platforms you want to modify.");
            return;
        }

        int boxCount = 0;
        int meshCount = 0;

        GameObject main = Selection.gameObjects[0];
        MeshFilter[] tr = main.GetComponentsInChildren<MeshFilter>();
        foreach (var mf in tr)
        {
            // 1. Clear existing colliders to avoid duplicates
            Collider[] existingColliders = mf.GetComponents<Collider>();
            foreach (var v in existingColliders)
            {
                if (v.isTrigger) continue;
                DestroyImmediate(v);
            }

            // 2. Determine the complexity based on the vertex count
            Mesh mesh = mf.sharedMesh;
            Debug.Log(mesh.vertexCount);
            if (mesh.vertexCount == VERTEX_THRESHOLD)
            {
                mf.gameObject.AddComponent<BoxCollider>();
                boxCount++;
            }
            else
            {
                
                MeshCollider mc = mf.gameObject.AddComponent<MeshCollider>();
                mc.sharedMesh = mesh;
                mc.convex = false;
                meshCount++;
            }

            EditorUtility.SetDirty(mf.gameObject);
        }

        Debug.Log($"Collider Assignment Complete: {boxCount} Box Colliders, {meshCount} Mesh Colliders assigned.");
    }
}
#endif