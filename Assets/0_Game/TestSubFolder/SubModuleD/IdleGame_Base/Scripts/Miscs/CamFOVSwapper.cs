using Cinemachine;
using LitMotion;
using UnityEngine;
[System.Serializable]
public class CamFOVSwapper : MonoBehaviour
{
    public SimpleTrussClimb m_climbState;
    public CinemachineVirtualCamera m_cam;
    private CinemachineFramingTransposer m_frame;
    

    public float m_newFOV;
    private float m_ogFOV;

    private MotionHandle m_handle;
    private void Awake()
    {
        m_frame = m_cam.GetCinemachineComponent<CinemachineFramingTransposer>();
       
        m_ogFOV = m_frame.m_CameraDistance;
        m_climbState.OnStateChanged += SetCamFOV;
    }

    void SetCamFOV(bool isClimbing)
    {
        float fov = isClimbing ? m_newFOV : m_ogFOV;

        m_handle.TryCancel();
        m_handle = LMotion.Create(m_frame.m_CameraDistance, fov, 1f).Bind(x => m_frame.m_CameraDistance = x).AddTo(m_cam);
   }
   
}
