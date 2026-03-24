using System.Collections.Generic;
using UnityEngine;

public class WaypointUtils : MonoBehaviour
{
    public static float m_randomOffset = 1;
    public static Vector3 CenterToTargetDir() { return Quaternion.Euler(0, Random.Range(0, 360f), 0f) * new Vector3(1, 0, 1).normalized; }

    public static Vector3 RayPlaneIntersect(Ray ray, float YPlaneValue)
    {
        Vector3 planeCenter = new Vector3(0, YPlaneValue, 0);
        float denom = Vector3.Dot(Vector3.up, ray.direction);
        if (Mathf.Abs(denom) > 0.0001f)
        {
            float t = Vector3.Dot(planeCenter - ray.origin, Vector3.up) / denom;
            if (t >= 0) return ray.GetPoint(t);
        }
        return Vector3.zero;
    }
    public static float XZDistance(Vector3 pos1, Vector3 pos2)
    {
        Vector3 diff = pos1 - pos2;
        diff.y = 0;
        return diff.magnitude;
    }
    public static int[] RandomKfromN(int k, int n)
    {
        if (k > n)
        {
            Debug.LogError("Cannot choose " + k + " elements out of " + n + ".");
        }

        Dictionary<int, int> hash = new Dictionary<int, int>(2 * k);
        int[] output = new int[k];

        for (int i = 0; i < k; i++)
        {
            int j = i + Random.Range(0, n - i);
            if (hash.ContainsKey(j))
            {
                output[i] = hash[j];
                hash.Remove(j);
            }
            else output[i] = j;
            if (j > i)
            {
                if (hash.ContainsKey(i))
                {
                    hash.Add(j, hash[i]);
                    hash.Remove(i);
                }
                else hash.Add(j, i);
            }
        }
        return output;
    }
}
