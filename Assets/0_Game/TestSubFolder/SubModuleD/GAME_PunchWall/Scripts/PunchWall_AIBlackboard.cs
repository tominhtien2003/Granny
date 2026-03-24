using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWall_AIBlackboard : MonoBehaviour
{
    public List<MachinesManager> m_machinesManager;
    public List<Transform> m_midWaypoints;
    public List<Transform> m_buyWaypoints;

    public Transform m_startPunchWaypoint;
    public Transform m_endPunchWaypoint;

    public WallStats m_wall;

    public List<AnimationClip> m_replacements;
}
