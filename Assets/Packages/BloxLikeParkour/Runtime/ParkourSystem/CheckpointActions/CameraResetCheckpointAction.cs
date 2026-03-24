using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResetCheckpointAction : MonoBehaviour, ICheckpointAction
{
    public CinemachineVirtualCamera m_cam;
    CinemachinePOV m_pov;
    void Awake()
    {
        m_pov = m_cam.GetCinemachineComponent<CinemachinePOV>();
    }
    public void CheckpointAction(CheckpointData data)
    {
        Quaternion q = Quaternion.LookRotation(transform.forward);
        m_pov.m_HorizontalAxis.Value = q.eulerAngles.y;
    }
}
