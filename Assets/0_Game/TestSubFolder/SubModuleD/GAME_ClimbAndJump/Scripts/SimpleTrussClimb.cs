using KinematicCharacterController;
using System;
using UnityEngine;
using UnityEngine.Events;
public interface ISpeedChanger
{
    public void SetSpeed(float multiplier);
    public void SetCurrentGlobalMultiplier(float multiplier);
}
public interface IStateChangeEvents
{
    public UnityAction<bool> OnStateChanged { get; set; }
}
public interface IOnStateUpdate
{
    public Action<float> OnStateUpdateAction { get; set; }
}
public interface IAutoClimb
{
    public void SetAuto(bool state);
    public UnityAction OnTired { get; set; }
    public void SetUnlimitedStamina();
}
public class SimpleTrussClimb : BaseMovementBloxState, ISpeedChanger, IStateChangeEvents,IAutoClimb
{
    private Vector3 _moveInputVector;
    public float m_climbSpeed;
    private float m_currentClimbSpeed;
    
    private LadderState m_ladderState;
    private float m_debugCos;
    public string VerticalSpeedParameter = "VerticalSpeed";
    public string IsBlimbingParameter = "IsClimbing";
    public float m_baseClimbSpeed;
    public bool JumpRequested => _jumpRequested;
    private bool _jumpRequested;
    public UnityAction<bool> OnStateChanged { get => m_onStateChanged; set => m_onStateChanged = value; }
    public UnityAction<bool> m_onStateChanged;

    public float m_currentGlobalMultiplier = 1f;
    public float m_maxStamina;
    private float m_currentStamina;
    private float m_currentTargetStamina;
    public BoxCollider CurrentTruss { get; set; }
   public UnityAction OnTired { get => m_onTired; set => m_onTired = value; }
    private UnityAction m_onTired;
    public GameObject[] m_lightningTrails;
    public float m_speedState;
    public int m_currentLightningTrails = -1;
    public GameObject m_lightningParent;

    public override void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        base.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);

    }
    public void SetSpeed(float multiplier)
    {
        m_climbSpeed = (m_baseClimbSpeed + multiplier) * m_currentGlobalMultiplier;
        SetTrails(m_baseClimbSpeed + multiplier);
    }
    private void SetTrails(float speed)
    {
        int id = Mathf.Min(Mathf.FloorToInt(Mathf.Log10(speed)), m_lightningTrails.Length) - 1;
        if (m_currentLightningTrails == id) return;
        if (m_currentLightningTrails >= 0) m_lightningTrails[m_currentLightningTrails].SetActive(false);
        if (id >= 0) m_lightningTrails[id].SetActive(true);
        m_currentLightningTrails = id;
    }
    public void SetCurrentGlobalMultiplier(float multiplier)
    {
        m_climbSpeed = m_climbSpeed * multiplier / m_currentGlobalMultiplier;
        m_currentGlobalMultiplier = multiplier;
    }
    public override void AfterCharacterUpdate(float deltaTime)
    {
        m_blackboard.Animator.SetFloat(VerticalSpeedParameter, Motor.BaseVelocity.y);
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public override void OnStateExit()
    {
         m_lightningParent.SetActive(false);
        if (m_ladderState == LadderState.LEAVING_TOP)
        {

            Vector3 pos = Motor.TransientPosition;
            pos.y = CurrentTruss.ClosestPoint(pos).y + .01f;
            Motor.SetPosition(pos + Motor.CharacterForward * .2f);
            Motor.BaseVelocity = Motor.CharacterForward * 1f;
            //this.LogError("LEAVING TOP " + pos.y + " " + Motor.TransientPosition);
            //Debug.Break();
        }
        else Motor.BaseVelocity = Vector3.zero;
            //else this.LogError("LEAVING");
            m_blackboard.Animator.SetBool(IsBlimbingParameter, false);
        OnStateChanged?.Invoke(false);
        SetTired(false, false);
    }
    private float m_trussHeight;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
         m_lightningParent.SetActive(true);
        SetTired(false, false);
        OnStateChanged?.Invoke(true);
        _jumpRequested = false;
        m_debugCos = 1;
        m_blackboard.Animator.SetBool(IsBlimbingParameter, true);
        m_trussHeight = CurrentTruss.bounds.extents.y + CurrentTruss.bounds.center.y;
        Motor.BaseVelocity = Vector3.zero;
        _moveInputVector = InputHolder.moveInputVector;

        m_currentStamina = 0;
        m_currentTargetStamina = m_maxStamina;

        if (m_climbSpeed < .1f)
        {
            UINotify.Instance.Notify("You are too slow!");
        }
    }
    public override void OnStateUpdate(float dt)
    {
        _moveInputVector = InputHolder.moveInputVector;
        if (InputHolder.JumpDown)
        {
            _jumpRequested = true;
            InputHolder.JumpDown = false;
        }

    }
    public bool CheckExit()
    {
        if (Motor.TransientPosition.y > m_trussHeight) m_ladderState = LadderState.LEAVING_TOP;
        else if (Motor.GroundingStatus.IsStableOnGround && m_debugCos < -.1f)
        {
            this.LogError(m_debugCos);
            m_ladderState = LadderState.LEAVING_BOTTOM;
        }
        else return false;
        return true;
    }



    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

    }
    public string NEED_TO_REST = "You need to rest for 5 seconds!";
    public string RESTED = "You feel rested!";
    public bool m_unlimitedStamina = false;
    public bool m_isTired = false;

    private float m_currentTimeLimit = 0;
    public void SetUnlimitedStamina()
    {
        m_unlimitedStamina = true;
        m_currentTimeLimit = 200;
        SetTired(false);
        
    }

    private float m_currentTiredTime = 0;
    void SetTired(bool isTired, bool notify = true)
    {
        m_currentTiredTime = 0;
        m_isTired = isTired;
        if (!isTired) this.Log("NOTIFYING");
        if (notify) UINotify.Instance.Notify(isTired? NEED_TO_REST:RESTED);
        m_currentStamina = 0;
    }

    private int m_autoStateSpeed = 0;
    public void SetAuto(bool state)
    {
        m_autoStateSpeed = state ? 1 : 0;
    }

    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        float vel = _moveInputVector.z;
        m_debugCos =vel;
        if (_moveInputVector.sqrMagnitude <= .001f)
        {
            vel = m_autoStateSpeed;
            m_debugCos = 0;
        }
        if (vel > 0.01f && Motor.GroundingStatus.FoundAnyGround) Motor.ForceUnground();
        currentVelocity = vel * (m_isTired?.3f: m_climbSpeed) * Vector3.up;
        if (m_unlimitedStamina)
        {
            m_currentTimeLimit -= deltaTime;
            if (m_currentTimeLimit <= 0) m_unlimitedStamina = false;
        }
        else if (!m_isTired)
        {
            m_currentStamina += vel * deltaTime;
            if (m_currentStamina >= m_currentTargetStamina)
            {
                OnTired?.Invoke();
                SetTired(true);
            }
        }
        else
        {
            if (vel < .01f) m_currentTiredTime += deltaTime;
            else m_currentTiredTime = 0;

            if (m_currentTiredTime >= 5f) SetTired(false);
            
        }
    }

}
