using BloxLikeBasic;
using Cinemachine;
using KinematicCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class PunchWallScript : MonoBehaviour, ISpeedChanger, IPetMultiplier
{
    public int m_currentWallId;
    public int m_currentWallHp;

    public WallStats m_wall;
    public bool CanPunchExternal = true;
    

    private float m_sqrMaxAutoAnimDistance;
    public KinematicCharacterMotor Motor;
    public List<double> m_wallHps;
    private List<double> m_maxWallHps;
    private List<double> m_ascensionData;
    [Header("Muscles")]
    public MuscleSetter m_muscleSetter;
   
    [Header("Reset")]
    public MapController m_mapController;
    public GameObject m_returnButton;
    public NormalMovement m_movement;
    private CupScript m_cup;

    [Header("Money")]
    public double m_gainedMoney;
    public MoneyController m_coins;
    public PunchWall_ValueReceiveAnimator m_moneyAnim;
    public double m_moneyMultiplier;

    [Header ("Animation")]
    public ParticleSystem m_particle;
    public ParticleSystem m_brokenParticle;
    public ParticleSystem m_smokeParticle;
    public ParticleSystem m_ascendEffect;
    public string[] m_punchAnims;
    private int m_currentPunchIndex;
    public float m_punchCooldown;
    private float m_currentCooldown;
    private bool m_canPunch;
    public Animator m_animator;
    private CinemachineImpulseSource m_cameraShake;

    [Header ("Damage and stats")]
    public double m_currentDamage;
    public double m_baseDamage;
    public PunchWall_ValueReceiveAnimator[] m_bodyPartPowerDisplay;
    public AudioClip m_rockBreakingSound;
    public double m_damageMultiplier = 1;
    public double m_trainingMultiplier;
    public double m_ascendTrainMultiplier;
    public double m_baseTrainAmount;

    private float m_ogMaxSpeed;
    private float m_ogAccel;


    private void Awake()
    {
        m_sqrMaxAutoAnimDistance = PunchWall_GlobalStatusHolder.MaxAutoAimDistance * PunchWall_GlobalStatusHolder.MaxAutoAimDistance;
        m_canPunch = true;
        m_cameraShake = GetComponent<CinemachineImpulseSource>();

        m_ogMaxSpeed = m_movement.MaxStableMoveSpeed;
        m_ogAccel = m_movement.StableMovementSharpness;
        m_cup = FindObjectOfType<CupScript>();
        m_muscleSetter = FindObjectOfType<MuscleSetter>();
    }
    private void Start()
    {
        m_maxWallHps = PunchWall_GlobalStatusHolder.Instance.TrueWallHps;
        m_wallHps = PunchWall_GlobalStatusHolder.Instance.CurrentWallHps;
        ResetWall();
        m_ascensionData = PunchWall_DataController.Instance.m_ascendRequirementData.m_values;
        UpdateBodyPartValues();
        UpdateBaseDamage();
        
        
    }
    public void PlayAscendEffect()
    {
        m_ascendEffect.transform.position = transform.position;
        m_ascendEffect.Play();
    }
    public void UpdateBodyPartValues()
    {
        for (int i = 0; i < 3; i++)
        {
            double d1 = PunchWall_DataController.m_bodypartStrength[i];
            double d2 = m_ascensionData[PunchWall_DataController.CurrentAscension];
            m_bodyPartPowerDisplay[i].SetValue(d1,d2);
            float prev = Mathf.Clamp((float)(PunchWall_DataController.m_bodypartStrength[i] / m_ascensionData[PunchWall_DataController.CurrentAscension]), .1f, 1f);
            m_muscleSetter.SetMusclesSize(prev, i);
        }
    }
    void ResetWall()
    {
        
        for (int i = 0; i < m_maxWallHps.Count; i++) m_wallHps[i] = m_maxWallHps[i];
        m_movement.MaxStableMoveSpeed = m_ogMaxSpeed;
        m_movement.StableMovementSharpness = m_ogAccel;
        m_currentWallId = 0;
        m_wall.gameObject.SetActive(true);
        SetWallInfo(m_currentWallId);
        m_wall.SetHp(m_wallHps[0], m_maxWallHps[0]);
        m_cup.EnableCollider(true);

    }
    void SetWallInfo(int id)
    {
        m_wallHps[id] = m_maxWallHps[id];
        PunchWall_GlobalStatusHolder.CurrentWall = id;
        m_wall.transform.localPosition = new Vector3(PunchWall_GlobalStatusHolder.Instance.WallStartOffset +
                    PunchWall_GlobalStatusHolder.Instance.WallDistance * id, 0, 0);
        m_wall.SetId(id);
    }
   
    public void LeavePunch()
    {
        PunchWall_GlobalStatusHolder.Punching = false;
    }
    void Update()
    {
        if (m_canPunch) return;
        m_currentCooldown += Time.deltaTime;
        if (m_currentCooldown >= m_punchCooldown)
        {
            m_currentCooldown = 0;
            m_canPunch = true;
        }
    }
    private bool CanPunchBreakWall = false;
    private string m_punchAnim;
    private Action OnPunchAction;
    private SpecialWall m_currentSPWall;
    public void Punch()
    {
        if (!m_canPunch || !CanPunchExternal) return;
        m_punchAnim = m_punchAnims[m_currentPunchIndex];

        bool hasSpWall = false;
        List<SpecialWall> specialWalls = PunchWall_SpecialWallManager.Instance.ActiveWalls;
        foreach (var v in specialWalls)
        {
            Vector3 diffs = v.transform.position - transform.position;
            diffs.y = 0;
            if (diffs.sqrMagnitude <= 25f)
            {
                Motor.RotateCharacter(Quaternion.LookRotation(diffs));
                CanPunchBreakWall = true;
                m_currentSPWall = v;
                if (m_currentDamage >= v.m_hp) m_punchAnim = "Clap";
                OnPunchAction = PunchSpecialWall;
                hasSpWall = true;
                break;
            }
            
            
        }
        if (!hasSpWall)
        {
            Vector3 diff = m_wall.transform.position - transform.position;
            diff.y = 0;
            if (m_currentWallId < m_wallHps.Count && diff.sqrMagnitude <= m_sqrMaxAutoAnimDistance)
            {
                Motor.RotateCharacter(Quaternion.LookRotation(-Vector3.right));
                CanPunchBreakWall = true;
                if (m_currentDamage >= m_wallHps[m_currentWallId]) m_punchAnim = "Clap";
                OnPunchAction = PunchBasicWall;
            }
        }
        //Play anim
        m_animator.CrossFadeInFixedTime(m_punchAnim, .1f);
        m_canPunch = false;
        m_currentPunchIndex = (m_currentPunchIndex + 1) % m_punchAnims.Length;
        

    }
    void PunchSpecialWall()
    {
        SpecialWall wall = m_currentSPWall;
        double damage = m_currentDamage;
        wall.m_hp -= damage;
        
        wall.m_stats.SetHp(wall.m_hp, wall.m_maxHp);
        m_particle.transform.position = wall.m_collider.ClosestPoint(transform.position + Vector3.up);
        m_particle.transform.rotation = Quaternion.LookRotation(transform.position - wall.transform.position);
        wall.m_stats.OnHit?.Invoke(-1);
        m_particle.Play();
        m_coins.AddToTotalMoney(m_gainedMoney, false);
        m_moneyAnim.ReceiveValue(m_coins.m_totalMoney, m_gainedMoney);

        if (wall.m_hp <= 0)
        {
            m_brokenParticle.Play();
            m_smokeParticle.Play();
            PunchWall_SpecialWallManager.Instance.ReturnWall(wall);
            
        }
    }
    void PunchBasicWall()
    {
        double damage = m_currentDamage;
        int cnt = 0;
        bool setted = false;
        for (int i = m_currentWallId; i < m_wallHps.Count; i++)
        {
            if (m_wallHps[i] > damage)
            {
                m_wallHps[i] -= (long)damage;
                m_wall.SetHp(m_wallHps[i], m_maxWallHps[i]);
                m_wall.OnHit?.Invoke(i);
                if (m_currentWallId != i)
                {
                    m_currentWallId = i;
                    SetWallInfo(i);
                }
                m_particle.transform.position = m_wall.transform.position + Vector3.up + Vector3.right * .5f;

                m_particle.Play();

                m_coins.AddToTotalMoney(m_gainedMoney, false);
                m_moneyAnim.ReceiveValue(m_coins.m_totalMoney, m_gainedMoney);
                if (cnt >= 10)
                {
                    StopAllCoroutines();
                    StartCoroutine(SpeedUp());
                }
                if (setted)
                {
                    m_brokenParticle.Play();
                    m_smokeParticle.Play();
                }
                AudioManager.Instance.PlayShot(m_rockBreakingSound);
                return;
            }

            damage -= m_wallHps[i];
            m_wall.OnBroken?.Invoke(i);
            if (!setted)
            {

                setted = true;
                m_brokenParticle.transform.position = m_wall.transform.position + Vector3.up - Vector3.right;
                m_smokeParticle.transform.position = m_brokenParticle.transform.position;

            }
            cnt++;
        }
        if (cnt >= 10)
        {
            StopAllCoroutines();
            StartCoroutine(SpeedUp());
        }
        if (setted)
        {
            m_brokenParticle.Play();
            m_smokeParticle.Play();
        }
        AudioManager.Instance.PlayShot(m_rockBreakingSound);
        m_coins.AddToTotalMoney(m_gainedMoney, false);
        m_moneyAnim.ReceiveValue(m_coins.m_totalMoney, m_gainedMoney);
        m_currentWallId = m_wallHps.Count;
        PunchWall_GlobalStatusHolder.CurrentWall = m_currentWallId;
        m_wall.gameObject.SetActive(false);
    }
    [Button]
    public void PunchCurrentWall()
    {
        if (!CanPunchBreakWall) return;
        CanPunchBreakWall = false;
        m_cameraShake.GenerateImpulse();
        OnPunchAction();

    }
    private IEnumerator SpeedUp()
    {
        m_movement.MaxStableMoveSpeed = 60;
        m_movement.StableMovementSharpness = 600;
        yield return Yielder.Get(10f);
        m_movement.MaxStableMoveSpeed = 30;
        m_movement.StableMovementSharpness = 300;
        yield return Yielder.Get(5f);
        m_movement.MaxStableMoveSpeed = m_ogMaxSpeed;
        m_movement.StableMovementSharpness = m_ogAccel;

    }
    public void UpdateTrain(int id)
    {
        double d = m_baseTrainAmount * m_trainingMultiplier * PunchWall_DataController.Instance.TrainMultiplier;
        double ascensionData = m_ascensionData[PunchWall_DataController.CurrentAscension];
        PunchWall_DataController.m_bodypartStrength[id] += d;

        PunchWall_GlobalStatusHolder.OnTrain?.Invoke();

        float aft = Mathf.Clamp((float)(PunchWall_DataController.m_bodypartStrength[id] / ascensionData), .1f, 1f);
        PunchWall_GlobalStatusHolder.Instance.CheckBodypartRequirement(id);

        m_muscleSetter.SetMusclesSize(aft, id);
        m_bodyPartPowerDisplay[id].ReceiveValue(PunchWall_DataController.m_bodypartStrength[id], d, ascensionData);
        UpdateBaseDamage();


    }
    public void UpdateBaseDamage()
    {
        double dam = 0;
        for (int i = 0; i < 3; i++) dam += PunchWall_DataController.m_bodypartStrength[i];
        m_baseDamage = dam;

        UpdateRealDamage();
    }
    public void UpdateRealDamage()
    {
        m_currentDamage = m_baseDamage * m_damageMultiplier;
        m_gainedMoney = Math.Round(m_currentDamage * .1f * m_moneyMultiplier);
    }
    public void SetSpeed(float multiplier)
    {
        m_damageMultiplier = multiplier;
        UpdateBaseDamage();
    }

    public void SetCurrentGlobalMultiplier(float multiplier)
    {
        
    }

    public void ChangeMultiplier(double multiplier)
    {
        m_trainingMultiplier += multiplier;
        if (m_trainingMultiplier < 0) m_trainingMultiplier = 1;
    }

    public void ReturnToLobby()
    {
        m_mapController.ResetPlayer();
        SetReturnState(false);
    }

    private bool m_returnState;
    public void SetReturnState(bool state)
    {
        if (m_returnState == state) return;
        m_returnState = state;
        m_returnButton.SetActive(state);
        if (!state)
        {
            if (PunchWall_GlobalStatusHolder.CanReturn) ResetWall();
            StopAllCoroutines();
        }
    }
    public void ToggleReturnState()
    {
        SetReturnState(!m_returnState);
    }

    public void AnimateMuscle()
    {
        m_muscleSetter.AnimateMuscleGroup(PunchWall_GlobalStatusHolder.CurrentBodypartId);
    }
}
