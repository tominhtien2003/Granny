using UnityEngine;

public class CheckpointData
{
    public int CheckpointId;
    public WaypointScript SpawnWaypoint;
    public Transform m_transform;
    public CheckpointData(int id, WaypointScript script, Transform transform)
    {
        CheckpointId = id;
        SpawnWaypoint = script;
        m_transform = transform;
    }
} 
