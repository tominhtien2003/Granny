using UnityEngine;
public class FanPhysicMover : MonoBehaviour
{
    public Vector3 LocalDirection;
    public bool affectGround = true;
    public bool affectAir = true;
    public float Speed = 3f;

    public Vector3 GetVelocity()
    {
        return transform.TransformDirection(LocalDirection.normalized) * Speed;
    }
}
