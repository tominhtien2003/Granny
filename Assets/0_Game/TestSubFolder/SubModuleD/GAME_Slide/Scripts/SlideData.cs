using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideData : MonoBehaviour
{
    public Transform m_slideStart;
    public Transform m_slideEnd;
    public Transform m_climbStart;
    public Transform m_climbEnd;

    public Transform m_slideLandPoint;

    public List<Transform> m_waypoints;
    public List<Transform> m_shopWaypoints;
    public Vector3 GetClosestPointOnSlide(Vector3 pos)
    {
        return Utils.GetClosestPointOnLine(pos, m_slideStart.position, m_slideEnd.position);
    }

    public Vector3 GetSlideDirection()
    {
        return (m_slideEnd.position - m_slideStart.position).normalized;
    }
    public Vector3 GetClimbDirection()
    {
        return (m_climbEnd.position - m_climbStart.position).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color= Color.yellow;
        foreach (var v in m_waypoints) if (v != null) Gizmos.DrawSphere(v.position, .5f);

        Gizmos.color = Color.blue;
        foreach (var v in m_shopWaypoints) if (v != null) Gizmos.DrawSphere(v.position, .5f);
    }
}
