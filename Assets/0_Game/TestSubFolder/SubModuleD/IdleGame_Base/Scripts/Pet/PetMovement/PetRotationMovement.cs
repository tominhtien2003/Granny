using KinematicCharacterController;
using LitMotion;
using LitMotion.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class PetRotationMovement : MonoBehaviour
{
    public float m_rotationSpeed = 5f;
    public List<Transform> m_currentPets;
    public List<MotionHandle> m_positionMotionHandles = new List<MotionHandle>();
    public List<MotionHandle> m_rotationMotionHandles = new List<MotionHandle>();
    public List<Transform> m_idlePoints;
    public List<Transform> m_rotatePoints;
    public List<Transform> m_rotateControls;
    public SimpleTrussClimb m_climbState;
    public KinematicCharacterMotor m_motor;

    private bool m_updating = false;

    public bool isClimb = true;
    private void Start()
    {
        for (int i = 0; i < m_currentPets.Count; i++)
        {
            m_positionMotionHandles.Add(new MotionHandle());
            m_rotationMotionHandles.Add(new MotionHandle());
        }
        MovePetsTo(m_idlePoints);
        
    }
    void SetState(bool state)
    {
        if (m_updating == state) return;
        m_updating = state;

        MovePetsTo(state?m_rotatePoints:m_idlePoints);
    }
    private void Update()
    {
        if (isClimb)
        {
            ClimbPetEffect();
        }
        else
        {
            RunningPetEffect();
        }
    }

    private void ClimbPetEffect()
    {
        SetState(transform.position.y > 5f && !m_motor.GroundingStatus.FoundAnyGround);
        if (!m_updating) return;
        if (m_motor.Velocity.y < 0.1f) return;
        foreach (Transform rotateControl in m_rotateControls)
        {
            rotateControl.localEulerAngles += Vector3.up * m_rotationSpeed * Time.deltaTime;
        }
    }

    private void RunningPetEffect()
    {
        Vector3 v = m_motor.BaseVelocity;
        v.y = 0;
        SetState(v.sqrMagnitude > .1f);
        if (!m_updating) return;
        if (v.sqrMagnitude < 0.1f) return;
        foreach (Transform rotateControl in m_rotateControls)
        {
            rotateControl.localEulerAngles += Vector3.up * m_rotationSpeed * Time.deltaTime;
        }
    }
    

    void MovePetsTo(List<Transform> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            m_positionMotionHandles[i].TryCancel();
            m_rotationMotionHandles[i].TryCancel();
            //m_currentPets[i].DOKill();
            m_currentPets[i].SetParent(points[i]);


            m_positionMotionHandles[i] = LMotion.Create(m_currentPets[i].localPosition, Vector3.zero, .5f).WithEase(Ease.OutQuad).BindToLocalPosition(m_currentPets[i]).AddTo(m_currentPets[i]);
            m_rotationMotionHandles[i] = LMotion.Create(m_currentPets[i].localEulerAngles, Vector3.zero, .5f).WithEase(Ease.OutQuad).BindToLocalEulerAngles(m_currentPets[i]).AddTo(m_currentPets[i]);
            /*m_currentPets[i].DOLocalMove(Vector3.zero, .5f).SetEase(Ease.OutQuad);
            m_currentPets[i].DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutQuad);*/
        }
    }
}
