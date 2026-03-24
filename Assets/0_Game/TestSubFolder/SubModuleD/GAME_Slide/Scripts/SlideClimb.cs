using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SlideClimb : BaseMovementBloxState, ISpeedChanger, IStateChangeEvents, IAutoClimb//, IOnStateUpdate
{  
    public float m_climbSpeed;

    public float m_baseClimbSpeed;
    public UnityAction<bool> OnStateChanged { get => m_onStateChanged; set => m_onStateChanged = value; }
    public UnityAction<bool> m_onStateChanged;

    public float m_currentGlobalMultiplier = 1f;
    public float m_temporalMultiplier = 1f;
    public float m_maxStamina;
    private float m_currentStamina;
    private float m_currentTargetStamina;
    public UnityAction OnTired { get => m_onTired; set => m_onTired = value; }
    public Action<float> OnStateUpdateAction { get; set; }
    private UnityAction m_onTired;
    public Image m_staminaImage;
    public GameObject m_staminaParent;
    public string PlanarSpeedParameter = "PlanarSpeed";
    public Collider m_triggerEnterColl;
    public GameObject[] m_lightningTrails;
    public float m_speedState;
    public int m_currentLightningTrails = -1;
    public GameObject m_lightningParent;
    public Vector3 m_overrideUp = Vector3.up;
    public float m_bottomPos = .02f;
    public void SetSpeed(float multiplier)
    {
        m_climbSpeed = (m_baseClimbSpeed + multiplier) * m_currentGlobalMultiplier * m_temporalMultiplier;
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
        m_blackboard.Animator.SetFloat(PlanarSpeedParameter, Mathf.Abs(Motor.BaseVelocity.x));
    }

    public override void OnStateExit()
    {
        OnStateChanged?.Invoke(false);
        m_blackboard.Animator.SetBool("Grounded", true);
        m_lightningParent.SetActive(false);
        Motor.BaseVelocity = Vector3.zero;
        Motor.SetGroundSolvingActivation(true);
        m_staminaParent.SetActive(false);
        SetTired(false, false);
        m_triggerEnterColl.enabled = true;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SetTired(false, false);
        OnStateChanged?.Invoke(true); m_lightningParent.SetActive(true);
        _moveInputVector = InputHolder.moveInputVector;
        m_currentStamina = m_maxStamina;
        m_staminaImage.fillAmount = 1;
        m_staminaParent.SetActive(true);
        if (m_climbSpeed < .1f)
        {
            UINotify.Instance.Notify("You are too slow!");
        }
        Motor.SetGroundSolvingActivation(false);
        Motor.ForceUnground();
        Motor.SetRotation(Quaternion.Euler(0, -90, 0));
        m_blackboard.Animator.SetBool("Grounded", true);
        m_triggerEnterColl.enabled = false;
    }
    private Vector3 _moveInputVector;
    public override void OnStateUpdate(float dt)
    {
        _moveInputVector = InputHolder.moveInputVector;

    }
    public float m_topPos;
    public bool m_unlimitedStamina = false;
    public bool m_isTired = false;

    private float m_currentTimeLimit = 0;
    public void SetUnlimitedStamina()
    {
        m_unlimitedStamina = true;
        m_currentTimeLimit = 200;
        m_currentStamina = m_maxStamina;
        m_staminaImage.fillAmount = 1;
        SetTired(false);

    }
    void SetTired(bool isTired, bool notify = true)
    {
        m_isTired = isTired;
        if (!isTired) this.Log("NOTIFYING");
    
        m_currentStamina = 0;
    }
    public Vector3 m_climbDir = new Vector3(-1.73205080757f, 1, 0).normalized;
    private int m_autoStateSpeed = 0;
    public void SetAuto(bool state)
    {
        m_autoStateSpeed = state ? 1 : 0;
    }
    private float m_cachedVel;
    public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        float vel = _moveInputVector.z;
        if (Mathf.Abs(vel) <= .1f)
        {
            vel = m_autoStateSpeed;
        }
        m_cachedVel = vel;
        currentVelocity = (m_isTired ? .3f : m_climbSpeed) * vel * m_climbDir;
        if (m_unlimitedStamina)
        {
            m_currentTimeLimit -= deltaTime;
            if (m_currentTimeLimit <= 0) m_unlimitedStamina = false;
        }
        else if (!m_isTired)
        {
            m_currentStamina -= Mathf.Max(vel,0) * deltaTime;
            m_staminaImage.fillAmount = m_currentStamina / m_maxStamina;
            if (m_currentStamina <= 0)
            {
                OnTired?.Invoke();
                SetTired(true);
            }
        }
    }

    public override void BeforeCharacterUpdate(float deltaTime)
    {
        
    }

    public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (Mathf.Abs(m_cachedVel) > 0.1f)
        {
            // Smoothly interpolate from current to target look direction
            Vector3 smoothedLookInputDirection = Vector3.Slerp(m_blackboard.Motor.CharacterForward, 
                Vector3.right * (m_cachedVel > 0? -1 : 1)
                , 1 - Mathf.Exp(-20f * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, m_blackboard.Motor.CharacterUp);
        }
        Vector3 currentUp = currentRotation * Vector3.up;
        currentRotation = Quaternion.FromToRotation(currentUp, m_overrideUp) * currentRotation;
    }

    public bool IsAtTop()
    {
        if (Motor.TransientPosition.y >= m_topPos)
        {
            Motor.SetTransientPosition(new Vector3(Motor.TransientPosition.x - 2f, m_topPos + .5f, Motor.TransientPosition.z));
            return true;
        }
        else if (Motor.TransientPosition.y <= m_bottomPos && _moveInputVector.z < -.1f)
            {
            //Debug.Log(_moveInputVector.z + " " + "out");
                Motor.SetPosition(new Vector3(Motor.TransientPosition.x + 3f, .02f, Motor.TransientPosition.z));
                return true;
            }
        return false;

    }

    public void SetTemporalMultiplier(float multi)
    {
     
        m_maxStamina = m_maxStamina * multi / m_temporalMultiplier;
        m_climbSpeed = m_climbSpeed * multi / m_temporalMultiplier;
        m_temporalMultiplier = multi;
       
    }
}
