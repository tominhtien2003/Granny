using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereVisualizer : MonoBehaviour
{
    public float Radius;
    public Color Color = Color.white;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
