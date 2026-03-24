using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Transform = UnityEngine.Transform;
public enum PlaceTypeEnum
{
    NONE,
    CONNECT,
    CONSECUTIVE,
    MOVE
}
[CustomEditor(typeof(WaypointScript))]
public class WaypointScriptEditor : Editor
{
    static bool m_isDrawing = false;
    static bool m_isConnecting = false;
    Vector3 m_start;
    Vector3 m_end;
    RaycastHit m_hit;
    static PlaceTypeEnum m_penum;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        EditorGUILayout.HelpBox("Waypoint Controls", MessageType.Info);
        WaypointScript waypointScript = target as WaypointScript;
        if (GUILayout.Button("Generate Waypoint"))
        {
            waypointScript.CreateWaypoint();
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }

        }

        if (GUILayout.Button("Connect Waypoint (E)"))
        {
            m_isDrawing = true;
            m_isConnecting = true;
            m_penum = PlaceTypeEnum.CONNECT;
        } 
        
        if (GUILayout.Button("Consecutive Generate Waypoint (F)"))
        {
            m_isDrawing = true;
            m_penum = PlaceTypeEnum.CONSECUTIVE;
        }

        if (GUILayout.Button("Move Waypoint (G)"))
        {
            m_isDrawing = true;
            m_penum = PlaceTypeEnum.MOVE;
        }
      
    }

    protected virtual void OnSceneGUI()
    {
        WaypointScript waypointScript = (WaypointScript)target;
        if (m_penum == PlaceTypeEnum.NONE)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                m_isDrawing = true;
                switch (e.keyCode)
                {
                    case KeyCode.E:
                        m_isConnecting = true;
                        m_penum = PlaceTypeEnum.CONNECT;
                        break;
                    case KeyCode.F:
                        m_penum = PlaceTypeEnum.CONSECUTIVE;
                        break;
                    case KeyCode.G:
                        m_penum = PlaceTypeEnum.MOVE;
                        break;
                    default:
                        m_isDrawing = false; 
                        break;
                }

            }
            return;
        }
        
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        m_start = waypointScript.transform.position;
        m_end = WaypointUtils.RayPlaneIntersect(ray, waypointScript.transform.position.y);
        
        if (m_penum == PlaceTypeEnum.CONNECT)
        {
           
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                m_isDrawing = false;
                m_penum = PlaceTypeEnum.NONE;
                return;
            }
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                m_isDrawing = false;

                if (m_hit.collider.transform && m_hit.collider.transform.GetComponent<WaypointScript>())
                {
                    if (m_penum == PlaceTypeEnum.CONNECT) waypointScript.AddNextWaypoint(m_hit.collider.transform);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(target);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                }
                waypointScript.CleanMissingConnection();
                m_penum = PlaceTypeEnum.NONE;
                int id = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = id;
                Event.current.Use();
                return;
            }

            if (Physics.Raycast(ray, out m_hit, Mathf.Infinity, Physics.AllLayers))
            {
                m_end = m_hit.point;
            }
            
            
        }

        if (m_penum == PlaceTypeEnum.CONSECUTIVE)
        {
          
            if (Physics.Raycast(ray, out m_hit, Mathf.Infinity, Physics.AllLayers))
            {
                m_end = m_hit.point;
                waypointScript.m_debugEnd = m_hit.point;
            }
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                m_isDrawing = false;
                m_penum = PlaceTypeEnum.NONE;
                return;
            }
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                waypointScript.CleanMissingConnection();
                GameObject g = waypointScript.CreateWaypoint();
                g.transform.position = m_end;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                int id = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = id;
                Event.current.Use();

                Selection.activeGameObject = g;

                return;
            }


        }
        if (m_penum == PlaceTypeEnum.MOVE)
        {
          
            if (Physics.Raycast(ray, out m_hit, Mathf.Infinity, Physics.AllLayers))
            {
                m_end = m_hit.point;
                waypointScript.m_debugEnd = m_hit.point;
            }
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                m_isDrawing = false;
                m_penum = PlaceTypeEnum.NONE;
                return;
            }
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                waypointScript.transform.position = m_end;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                int id = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = id;
                Event.current.Use();

                return;
            }


        }
        waypointScript.m_debugStart = m_start;
        waypointScript.m_debugEnd = m_end;
        SceneView.RepaintAll();
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
    static void DrawGizmos(WaypointScript waypointScript, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        foreach (Transform nextPoint in waypointScript.m_nextWaypoints)
        {
            if (nextPoint != null) Gizmos.DrawLine(waypointScript.transform.position + Vector3.up * 0.3f, nextPoint.position + Vector3.up * 0.3f);
        }

        if (waypointScript.m_useCustomColor) Gizmos.color = waypointScript.m_customColor;
        else Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(waypointScript.transform.position, waypointScript.m_overrideRandomOffset > 0 ? waypointScript.m_overrideRandomOffset : WaypointUtils.m_randomOffset);

        
        if (((int)gizmoType & (int)GizmoType.Selected) == 0) return;
       
        if (!m_isDrawing) return;
        if (m_penum == PlaceTypeEnum.MOVE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(waypointScript.m_debugEnd, WaypointUtils.m_randomOffset);
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(waypointScript.m_debugStart, waypointScript.m_debugEnd);
        if (m_penum == PlaceTypeEnum.CONSECUTIVE)
        {
            Gizmos.DrawSphere(waypointScript.m_debugEnd, WaypointUtils.m_randomOffset);
        }
       

    }
}
