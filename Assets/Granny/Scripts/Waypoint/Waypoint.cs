using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> Neighbours = new List<Waypoint>();

    public Waypoint GetNeighbour()
    {
        if (Neighbours.Count == 0) return  null;
        if (Neighbours.Count == 1) return Neighbours[0];
        
        int randomIndex = Random.Range(0, Neighbours.Count);
        return Neighbours[randomIndex];
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.4f);

        if (Neighbours == null) return;

        Gizmos.color = Color.cyan;
        foreach (var neighbor in Neighbours)
        {
            if (neighbor != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }
    }
    #endif
}
