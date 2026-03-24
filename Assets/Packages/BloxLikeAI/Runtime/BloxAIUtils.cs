using UnityEngine;

public static class BloxAIUtils
{
    public static void GetDistance(Vector3 position, Vector3 destination, out float horizontalDistance, out float verticalDistance)
    {
        Vector3 dis = destination - position;
        verticalDistance = Mathf.Abs(dis.y);
        dis.y = 0;
        horizontalDistance = Vector3.Magnitude(dis);
    }
}
