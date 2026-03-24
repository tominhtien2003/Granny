using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public enum WaypointType
{
    MOVE,
    SPAWN,
    WAIT,
    SELF_WAIT,
    OFF
}
public class WaypointScript : MonoBehaviour
{
    public List<Transform> m_nextWaypoints = new List<Transform>();
   
    //public WaypointType m_waypointType;

    public float m_overrideRandomOffset = -1;
    public float m_overrideWait = -1;
    public float m_overrideMargins = -1;


    public bool CanMove;
    public void SetCanMove(bool move)
    {
        CanMove = move;
    }
    protected virtual void Start()
    {
        if (m_nextWaypoints == null) m_nextWaypoints = new List<Transform>();  
    }
    public void AddNextWaypoint(Transform r)
    {
        if (m_nextWaypoints == null) m_nextWaypoints = new List<Transform>();
        if (!m_nextWaypoints.Contains(r)) m_nextWaypoints.Add(r);
        
    }
    public float GetRandomRadius()
    {
        if (m_overrideRandomOffset < 0) return WaypointUtils.m_randomOffset;
        else return m_overrideRandomOffset;
    }
#if UNITY_EDITOR
    public Vector3 m_debugStart;
    public Vector3 m_debugEnd;


    public bool m_useCustomColor;
    public Color m_customColor;
    public void CleanMissingConnection()
    {
        for (int i= 0; i < m_nextWaypoints.Count; i++)
        {
            if (m_nextWaypoints[i] == null) m_nextWaypoints.RemoveAt(i); 
        }
    }
    public GameObject CreateWaypoint()
    {
        GameObject g = null;
        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
        if (prefab == null)
        {
            prefab = gameObject;
            g = Instantiate(prefab);
        }
        else
        {
            g = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        }
        g.name = "Waypoint";
        if (transform.parent != null) g.transform.parent = transform.parent;
        g.transform.localScale = transform.localScale;
        g.transform.localPosition = transform.localPosition;
        AddNextWaypoint(g.transform);
        GameObject[] newSelection = new GameObject[1];
        newSelection[0] = g;
        Selection.objects = newSelection;
        return g;
    }

    protected virtual void OnDrawGizmos()
    {
        

        
    }
    public void DeleteWaypoint()
    {
        Destroy(gameObject);
    }

#endif



}
