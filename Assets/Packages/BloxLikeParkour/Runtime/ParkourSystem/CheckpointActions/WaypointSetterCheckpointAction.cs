using UnityEngine;

public class WaypointSetterCheckpointAction : MonoBehaviour, ICheckpointAction
{
    public BloxAI_FollowWaypoints m_followWaypoints;

    public void CheckpointAction(CheckpointData data)
    {
        m_followWaypoints.AssignWaypoint(data.SpawnWaypoint);
    }
}