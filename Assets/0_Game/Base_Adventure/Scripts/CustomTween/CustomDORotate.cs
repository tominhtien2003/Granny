using UnityEngine;
using PrimeTween;
using KinematicCharacterController;
using UnityEngine.UIElements;
public class CustomDORotate : MonoBehaviour, IMoverController
{
    private Rigidbody m_rb;
    public float m_rotateTime = 3f;
    public Vector3 m_rotateAround = Vector3.up;
    public float m_rotateAngle;
    public float m_startDelay = 0f;
    public float m_currentAngle = 0f;
    public Ease m_rotateEase = Ease.Linear;
    public CycleMode m_rotateCycleMode = CycleMode.Yoyo;

    public PhysicsMover Mover;

    private Transform _transform;
    private Vector3 _goalPosition;
    private Quaternion _goalRotation;

    private Quaternion _initialRotation;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        _transform = transform;
        if(Mover != null)
        {
            Mover.MoverController = this;
        }
        _initialRotation = _transform.rotation;
        _goalPosition = _transform.position;
        _goalRotation = _initialRotation;
        StartRotateTween();
    }

    void StartRotateTween()
    {
        Quaternion initialRotation = transform.rotation;

        Vector3 axis = m_rotateAround.normalized;

        Tween.Custom(0f, m_rotateAngle, m_rotateTime, onValueChange: angle =>
        {
            m_currentAngle = angle;

            Quaternion rotationDelta = Quaternion.AngleAxis(angle, axis);
            m_rb.rotation = initialRotation * rotationDelta;
            _goalRotation = m_rb.rotation;
        }, m_rotateEase, -1, m_rotateCycleMode).SetTarget(this);
    }
    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        goalPosition = _goalPosition; 
        goalRotation = _goalRotation;
    }
}
