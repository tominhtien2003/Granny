using BloxLikeBasic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Events;
public class Parkour_DeathEvent : MonoBehaviour
{
    public UnityAction<CharacterCheckpointData> OnDeath;
    public KinematicCharacterMotor Motor;
    public CharacterCheckpointData m_data;
    public bool FallDeath, StuckDeath;

    [SerializeField] private CheckStuckComponent m_checkStuckComponent;
    [SerializeField] float checkInterval = 1f;
    [SerializeField] float minDistancePerSecond = 1f;
    [SerializeField] float stuckTimeToDie = 3f;

    public void OnTriggerEnter(Collider other)
    {
        if (m_data.m_deathToggler.IsDead) return;
        if (other.gameObject.CompareTag("KillNoShield"))
        {
            if (ShieldComponent.Instance && ShieldComponent.isShield) return;
            m_data.m_deathToggler.IsDead = true;
            OnDeath?.Invoke(m_data);
            return;
        }
        if (other.gameObject.CompareTag("Kill"))
        {
            OnDeath?.Invoke(m_data);
            m_data.m_deathToggler.IsDead = true;
        }
    }
    private void Start()
    {
        m_checkStuckComponent = new CheckStuckComponent(transform, checkInterval, minDistancePerSecond, stuckTimeToDie);
        m_checkStuckComponent.Motor = Motor;
    }
    private void Awake()
    {
        foreach (var v in m_data.m_deathToggler.m_killableStates) v.OnWallHitEvent += CheckKillblock;
    }
    private void FixedUpdate()
    {
        if (FallDeath)
        {
            if (!m_data.m_deathToggler.IsDead && Motor.Velocity.y < -30f)
            {
                Debug.Log("Died to fall " + Motor.Velocity.y);
                m_data.m_deathToggler.IsDead = true;
                OnDeath?.Invoke(m_data);
            }

        }
        if (StuckDeath && m_checkStuckComponent != null && m_checkStuckComponent.CheckStuck(Time.fixedDeltaTime))
        {
            m_data.m_deathToggler.IsDead = true;
            OnDeath?.Invoke(m_data);
        }

    }
    void CheckKillblock(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint)
    {
        if (m_data.m_deathToggler.IsDead) return;
        if (hitCollider.gameObject.CompareTag("KillNoShield"))
        {
            if (ShieldComponent.Instance && ShieldComponent.isShield) return;
            m_data.m_deathToggler.IsDead = true;
            OnDeath?.Invoke(m_data);
            return;
        }
        if (hitCollider.gameObject.CompareTag("Kill"))
        {
            m_data.m_deathToggler.IsDead = true;
            OnDeath?.Invoke(m_data);
        }
    }
    public void Death()
    {
        if (m_data.m_deathToggler.IsDead) return;
        m_data.m_deathToggler.IsDead = true;
        OnDeath?.Invoke(m_data);
    }
}
