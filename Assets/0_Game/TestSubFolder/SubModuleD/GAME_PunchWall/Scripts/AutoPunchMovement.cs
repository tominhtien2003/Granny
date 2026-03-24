using BloxLikeBasic;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPunchMovement : MonoBehaviour
{
    public Vector3 m_startDir;
    public Vector3 m_endDir;

    public NormalMovement m_normalMovementStats;
    public KinematicCharacterMotor Motor;

    public PunchWallScript m_punchWallScript;

    private Vector3 m_diff;
    [Button]
    public void StartAutoPunch()
    {
        if (this.enabled) return;
        Motor.SetPosition(Utils.GetClosestPointOnLine(Motor.TransientPosition, m_startDir, m_endDir));
        m_diff = (m_endDir - m_startDir).normalized;
        m_diff.y = 0;
        this.enabled = true;
    }
    public void StopAutoPunch()
    {
        this.enabled = false;
    }
    private void FixedUpdate()
    {
        Motor.SetTransientPosition(Motor.TransientPosition + m_normalMovementStats.MaxStableMoveSpeed * Time.fixedDeltaTime * m_diff);
        if (Vector3.SqrMagnitude(m_endDir - Motor.TransientPosition) < 4f)
        {
            StopAutoPunch();
            return;
        }
        if (m_punchWallScript.m_currentWallId >= PunchWall_GlobalStatusHolder.Instance.WallHps.Count) return;
        if (Mathf.Abs(Motor.TransientPosition.x - m_punchWallScript.m_wall.transform.position.x) < 1f)
        {
            m_punchWallScript.Punch();
        }

    }
}
