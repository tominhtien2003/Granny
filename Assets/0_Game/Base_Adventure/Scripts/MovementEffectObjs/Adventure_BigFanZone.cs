using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_BigFanZone : MonoBehaviour
{
    [Header("Fan Settings")]
    public float hoverHeight = 8f;
    public float liftSpeed = 12f;

    public float hoverDamping = 10f;
    public float oscillation = 0.3f;
    public float oscillationSpeed = 2.5f;

    public void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.up * hoverHeight, Color.blue);
    }
}
