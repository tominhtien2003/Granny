using KinematicCharacterController;
using UnityEngine;

public class PositionCheckpointAction:MonoBehaviour, ICheckpointAction
{
    public KinematicCharacterMotor Motor;

    public void CheckpointAction(CheckpointData data)
    {
        Vector3 offset = Random.insideUnitSphere;
        offset.y = 0;
        offset *= Random.Range(.5f, 1f);
        Motor.SetPosition(data.m_transform.position + Vector3.up * 2f + offset);
        Motor.SetRotation(Quaternion.LookRotation(Vector3.ProjectOnPlane(data.m_transform.forward, Vector3.up), Vector3.up));
    }
}
