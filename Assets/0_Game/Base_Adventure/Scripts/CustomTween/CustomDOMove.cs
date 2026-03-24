using PrimeTween;
using UnityEngine;
using System.Collections;
using KinematicCharacterController.Walkthrough.MovingPlatform;
using KinematicCharacterController;

public enum MoveProgress
{
    AT_START,
    AT_END,
    MOVING
}

public class CustomDOMove : MonoBehaviour, IMoverController
{
    public Transform m_start;
    public Transform m_end;

    public float m_waitTime = 0.01f;
    public float m_moveSpeed;
    public bool m_useSelfAsStart;
    public bool m_useOffset;
    public Vector3 m_offset;
    private Vector3 m_selfStartPos;
    private Rigidbody m_rb;

    public float m_percentStart;
    public float m_percentEnd;

    public MoveProgress m_moveProgress;

    private Vector3 m_startPos;
    private Vector3 m_endPos;

    public float m_startDelay;
    private float m_dis;


    public PhysicsMover Mover;

    private Transform _transform;
    private Vector3 _goalPosition;
    private Quaternion _goalRotation;

    private Quaternion _initialRotation;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();

        m_selfStartPos = transform.position;

        if (m_useSelfAsStart)
            m_startPos = m_selfStartPos;
        else
            m_startPos = m_start.position;

        if (m_useOffset)
            m_endPos = m_startPos + m_offset;
        else
            m_endPos = m_end.position;

        m_dis = Vector3.Distance(m_startPos, m_endPos);
        _transform = transform;
        if (Mover != null)
        {
            Mover.MoverController = this;
            _initialRotation = Mover.Rigidbody.rotation;
            _goalPosition = Mover.Rigidbody.position;
        }
        _goalRotation = _initialRotation;
        StartCoroutine(StartMove());
    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(m_startDelay);
        Sequence.Create()
        .Chain(Tween.Custom(m_startPos, m_endPos, m_dis / m_moveSpeed, pos => _goalPosition = pos, Ease.Linear))
        .ChainDelay(m_waitTime)                          
        .Chain(Tween.Custom(m_endPos, m_startPos, m_dis / m_moveSpeed, pos => _goalPosition = pos, Ease.Linear))
        .ChainDelay(m_waitTime)                          
        .SetRemainingCycles(-1);
    }

    void Update()
    {
        float percentage = Vector3.Distance(transform.position, m_startPos) / m_dis * 100f;

        if (percentage <= m_percentStart)
            m_moveProgress = MoveProgress.AT_START;
        else if (percentage >= m_percentEnd)
            m_moveProgress = MoveProgress.AT_END;
        else
            m_moveProgress = MoveProgress.MOVING;
    }
    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        goalPosition = _goalPosition;
        goalRotation = _initialRotation;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (m_useOffset) Gizmos.DrawSphere(transform.position + m_offset, .3f);
    }
}
