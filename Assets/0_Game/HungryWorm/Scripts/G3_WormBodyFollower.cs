using System.Collections.Generic;
using UnityEngine;

public class G3_WormBodyFollower : MonoBehaviour
{
    [Header("Body Segments")]
    [SerializeField] private List<Transform> segments = new List<Transform>();

    [Header("Follow Settings")]
    [SerializeField] private float segmentSpacing = 0.6f;
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Head Sway (Optional)")]
    public Transform headVisual;
    [SerializeField] private float headSwaySpeed = 4f;
    [SerializeField] private float headSwayAngle = 3f;

    [Header("Stability")]
    [SerializeField] private float recordDistance = 0.05f;
    [SerializeField] private int maxPathPoints = 600;

    private readonly List<Vector3> headPath = new List<Vector3>();

    void LateUpdate()
    {
        RecordHeadPath();
        UpdateSegments();
        UpdateHeadSway();
    }

    void RecordHeadPath()
    {
        if (headPath.Count == 0 ||
            Vector3.Distance(headPath[0], transform.position) >= recordDistance)
        {
            headPath.Insert(0, transform.position);
        }

        if (headPath.Count > maxPathPoints)
            headPath.RemoveAt(headPath.Count - 1);
    }

    void UpdateSegments()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Transform seg = segments[i];
            if (seg == null) continue;

            float targetDistance = segmentSpacing * (i + 1);
            Vector3 targetPos = GetPointOnPath(targetDistance);

            seg.position = Vector3.Lerp(
                seg.position,
                targetPos,
                followSpeed * Time.deltaTime
            );

            Vector3 dir = targetPos - seg.position;
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                seg.rotation = Quaternion.Slerp(
                    seg.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
    void UpdateHeadSway()
    {
        if (headVisual == null) return;

        float sway = Mathf.Sin(Time.time * headSwaySpeed) * headSwayAngle;
        headVisual.localRotation = Quaternion.Euler(0f, sway, 0f);
    }

    Vector3 GetPointOnPath(float distance)
    {
        float travelled = 0f;

        for (int i = 0; i < headPath.Count - 1; i++)
        {
            float d = Vector3.Distance(headPath[i], headPath[i + 1]);

            if (travelled + d >= distance)
            {
                float t = (distance - travelled) / d;
                return Vector3.Lerp(headPath[i], headPath[i + 1], t);
            }

            travelled += d;
        }

        return headPath.Count > 0
            ? headPath[headPath.Count - 1]
            : transform.position;
    }
}
