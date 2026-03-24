using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;
[System.Serializable]
public struct WPInfo
{
    public Vector3 pos;
    public int id;
    public Transform tr;
    public WPInfo(Vector3 pos, int id, Transform tr)
    {
        this.pos = pos;
        this.id = id;
        this.tr = tr;
    }
}
public class WaypointGenerator : MonoBehaviour
{
    public float m_minArea;
    public float m_maxArea;
    public List<WPInfo> m_wpInfos;
    public List<string> m_namesToIgnore;
    public List<string> m_tagsToIgnore;
    public int[] m_path;
    public WaypointScript m_waypointPrefab;
    public float m_maxDetectNodeRange = 2f;
    public List<WaypointScript> m_wpLists = new List<WaypointScript>();
    public bool m_drawGizmos = false;
#if UNITY_EDITOR
    public void PrepareWaypoints()
    {
        if (m_wpInfos == null) m_wpInfos = new List<WPInfo>();
        m_wpInfos.Clear();
        Collider[] colls = GetComponentsInChildren<Collider>();
        int cnt = 0;
        for (int i = 0; i < colls.Length; i++)
        {
            var v = colls[i];
            if (v.isTrigger) continue;
            bool ch = false;
            foreach (string s in m_namesToIgnore) {
                if (v.gameObject.name.Contains(s))
                {
                    ch = true;
                    break;
                }
            }
            foreach (string s in m_tagsToIgnore) {
                if (v.gameObject.CompareTag(s))
                {
                    ch = true;
                    break;
                }
            }
            if (ch) continue;
            
            Bounds b = v.bounds;
            Vector3 diff = b.max - b.min;
            diff.y = 0;
            float area = diff.magnitude;

            if (area <= m_minArea || Mathf.Abs(diff.x) <= m_minArea || Mathf.Abs(diff.z) <= m_minArea) {
                Debug.Log("Not enough space", v);
                continue;
            }

            Vector3 cent = b.center;
            cent.y = b.max.y + 2f;
            if (v.Raycast(new Ray(cent, Vector3.down),out RaycastHit hit, 10f))
            {
                m_wpInfos.Add(new WPInfo(hit.point, cnt, v.transform));
                cnt++;
            }
        }
    }
    public void TryCreateWaypoints()
    {
        List<int> unvisited = new List<int>();
        List<float> values = new List<float>(new float[m_wpInfos.Count]);
        List<int> backtrack = new List<int>(new int[m_wpInfos.Count]);
        List<int> path = new List<int>();
        for (int i = 0; i < m_wpInfos.Count; i++)
        {
            unvisited.Add(i);
            values[i] = float.MaxValue;
        }

        values[0] = 0;
        while (unvisited.Count > 0)
        {
            float minDis = float.MaxValue;
            int chosen = -1;
            foreach(int i in unvisited)
            {
                if (values[i] < minDis)
                {
                    minDis = values[i];
                    chosen = i;
                }
            }
            
            if (chosen < 0 || chosen == m_wpInfos.Count - 1)
            {
                Debug.Log("Terminated at " + chosen);
                break;
            }

            minDis = float.MaxValue;
            int backup = -1;
            int backup2 = -1;
            unvisited.Remove(chosen);
            bool b = false;
            foreach (int i in unvisited)
            {
                float diff = (m_wpInfos[i].pos - m_wpInfos[chosen].pos).magnitude;

                if (diff > m_maxDetectNodeRange)
                {
                    float yDiff = Mathf.Abs(m_wpInfos[i].pos.y - m_wpInfos[chosen].pos.y);
                    if (diff < minDis)
                    {
                        minDis = diff;
                        if (yDiff <= 8f) backup = i;
                        backup2 = i;
                    }
                    continue;
                }
                b = true;
                float dis = values[chosen] + (m_wpInfos[i].pos - m_wpInfos[chosen].pos).magnitude;
                if (values[i] > dis)
                {
                    values[i] = dis;
                    backtrack[i] = chosen;
                    if (i == m_wpInfos.Count - 1) Debug.Log(i + " Min " + chosen);
                        
                }
            }
            if (!b)
            {
                int i = backup;
                if (i < 0) i = backup2;
                float dis = values[chosen] + (m_wpInfos[i].pos - m_wpInfos[chosen].pos).magnitude;
                
                if (values[i] > dis)
                {
                    values[i] = dis;
                    backtrack[i] = chosen;
                }
            }
            
        }

        int st = m_wpInfos.Count - 1;
        while (st != 0)
        {
            Debug.Log("backtrack " + st);
            path.Add(st);
            st = backtrack[st];
           
        }
        path.Add(0);
        m_path = path.ToArray();
        //Array.Reverse(m_path);
        
        
    }
    public void CleanAllWaypoints()
    {
        foreach (var v in m_wpLists) if (v != null) DestroyImmediate(v.gameObject);
        m_wpLists.Clear();
        EditorUtility.SetDirty(this);
    }
    public void InstantiateWaypoints()
    {
        foreach (var v in m_wpLists) if (v != null) DestroyImmediate(v.gameObject);
        m_wpLists.Clear();
        WaypointScript curr = null;
        foreach (var v in m_path)
        {
            var h = CreateWaypoint(v);
            if (curr != null) h.AddNextWaypoint(curr.transform);
            curr = h;
            m_wpLists.Add(h);
        }
        EditorUtility.SetDirty(this);
    }
    public void GetExistingWaypoints()
    {
        
        m_wpLists.Clear();
        var h = GetComponentsInChildren<WaypointScript>();
        foreach (var v in h) m_wpLists.Add(v);
        EditorUtility.SetDirty(this);
    }
    WaypointScript CreateWaypoint(int v)
    {
        GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(m_waypointPrefab.gameObject);
        g.name = "Waypoint";
        g.transform.parent = m_wpInfos[v].tr;
        g.transform.position = m_wpInfos[v].pos;
        return g.GetComponent<WaypointScript>();
    }
#endif
    private void OnDrawGizmos()
    {
        if (!m_drawGizmos) return;
        Gizmos.color = Color.red;
        foreach (var v in m_wpInfos)
        {
            Gizmos.DrawRay(v.pos, Vector3.up);
        }

        Gizmos.color = Color.green;
        for (int i= 1; i < m_path.Length; i++)
        {
            if (m_path[i] >= m_wpInfos.Count || m_path[i - 1] >= m_wpInfos.Count) continue;
            Gizmos.DrawLine(m_wpInfos[m_path[i]].pos + Vector3.up, m_wpInfos[m_path[i - 1]].pos + Vector3.up);
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(WaypointGenerator))]
public class WaypointGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WaypointGenerator gen = target as WaypointGenerator;
        EditorGUILayout.HelpBox("Waypoint Controls", MessageType.Info);
        if (GUILayout.Button("Try Generate"))
        {
            gen.PrepareWaypoints();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Try Prepare Waypoints"))
        {
            gen.TryCreateWaypoints();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("GetExistingWaypoints"))
        {
            gen.GetExistingWaypoints();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Isntantiate Waypoints"))
        {
            gen.InstantiateWaypoints();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Clean Waypoints"))
        {
            gen.CleanAllWaypoints();
            EditorUtility.SetDirty(target);
        }

    }
}
#endif