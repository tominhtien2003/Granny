using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    public void AssignCheckpoints()
    {
        if (!TryGetComponent<CheckpointManager>(out var manager)) return;
        List<GameObject> list = new List<GameObject>();
        Transform[] tr = GetComponentsInChildren<Transform>();
        foreach (var t in tr)
        {
            if (t.gameObject.name.Contains("Platform_Start"))
            {
                list.Add(t.gameObject);
                BoxCollider[] coll = t.GetComponents<BoxCollider>();
                bool b = false;
                BoxCollider bb = null;
                foreach (var c in coll)
                {
                    if (c.isTrigger)
                    {
                        bb = c;
                        b = true;
                    
                    }
                }
                if (!b)
                {
                    var bc = t.gameObject.AddComponent<BoxCollider>();
                    bc.isTrigger = true;
                    bc.center += Vector3.forward * 70f;
                    UnityEditor.EditorUtility.SetDirty(bc);
                }
                else
                {
                    Vector3 cent = bb.center;
                    cent.z = .2f;
                    bb.center = cent;

                }
                
            }
        }

        manager.Checkpoints = list.ToArray();
        UnityEditor.EditorUtility.SetDirty(manager);
        UnityEditor.EditorUtility.SetDirty(this);

    }

    public void AssignCheckpointTags()
    {
        Transform[] tr = GetComponentsInChildren<Transform>();
        foreach (var t in tr)
        {
            if (t.gameObject.name.Contains("Platform_Start"))
            {
                t.gameObject.tag = "Checkpoint";
                UnityEditor.EditorUtility.SetDirty(t);
            }
        }
    }
    public void ClearAllTriggerColliders()
    {
        Transform[] tr = GetComponentsInChildren<Transform>();
        foreach (var t in tr)
        {
            if (t.gameObject.name.Contains("Platform_Start"))
            {
                BoxCollider[] coll = t.GetComponents<BoxCollider>();
                bool b = false;
                BoxCollider bb = null;
                foreach (var c in coll)
                {
                    if (c.isTrigger)
                    {
                        bb = c;
                        b = true;
                        DestroyImmediate(c);
                    }
                }
                UnityEditor.EditorUtility.SetDirty(t);
            

            }
        }
    }
    public void GetCheckpointWaypoints()
    {
        List<WaypointScript> wpLists = GetComponent<WaypointGenerator>().m_wpLists;
     
        GameObject[] Checkpoints = GetComponent<CheckpointManager>().Checkpoints;
        GetComponent<CheckpointManager>().Waypoints = new List<WaypointScript>();
        List<WaypointScript> Waypoints = GetComponent<CheckpointManager>().Waypoints;
        Waypoints.Clear();
        for (int i = 0; i < Checkpoints.Length; i++) {
            float mini = float.MaxValue;
            WaypointScript chosen = null;
            foreach (var wp in wpLists) {
                if (wp == null) continue;
                float diff = 
                    Vector3.Distance(wp.transform.position, 
                    Checkpoints[i].transform.position);
                if (mini > diff)
                {
                    chosen = wp;
                    mini = diff;
                }
            }
            Waypoints.Add(chosen);
        }
        UnityEditor.EditorUtility.SetDirty(GetComponent<CheckpointManager>());


    }
    public void SnapWaypointsToCheckpoints()
    {
        GameObject[] Checkpoints = GetComponent<CheckpointManager>().Checkpoints;
        List<WaypointScript> Waypoints = GetComponent<CheckpointManager>().Waypoints;
        for (int i = 0; i < Checkpoints.Length; i++) {
         
            Vector3 pos1 = Checkpoints[i].GetComponent<Collider>().bounds.center;
            pos1.y = Checkpoints[i].GetComponent<Collider>().bounds.max.y;
            Waypoints[i].transform.position = pos1;

        }
        UnityEditor.EditorUtility.SetDirty(GetComponent<CheckpointManager>());


    }

#endif

}

#if UNITY_EDITOR
[CustomEditor(typeof(CheckpointGenerator))]
public class CheckpointGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CheckpointGenerator gen = target as CheckpointGenerator;
        if (GUILayout.Button("Try Generate"))
        {
            gen.AssignCheckpoints();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Clear all triggers"))
        {
            gen.ClearAllTriggerColliders();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Assign Tag"))
        {
            gen.AssignCheckpointTags();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Assign Checkpoint waypoints"))
        {
            gen.GetCheckpointWaypoints();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Snap Checkpoint waypoints"))
        {
            gen.SnapWaypointsToCheckpoints();
            EditorUtility.SetDirty(target);
        }
        

    }
}
#endif
