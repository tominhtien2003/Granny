using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.LowLevel;

public abstract class G3_BotChase : MonoBehaviour
{
    [Header("Base References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public float attackRange = 2f;
    public Transform parent;
    public bool canChase = true;
    protected BotState state = BotState.Chase;

    protected Parkour_DeathEvent playerDeathEvent;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool initialCanChase;
    public CinemachineVirtualCamera camAttack;

    protected virtual void Start()
    {
        agent.stoppingDistance = attackRange;
        agent.speed = 3.5f;

        parent = transform.parent;
        playerDeathEvent = player.GetComponent<Parkour_DeathEvent>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialCanChase = true;
        G3_Manager.Instance.cpManager.OnPlayerRespawnEvent += ResetBot;
    }

    protected virtual void Update()
    {
        if (!canChase)
        {
            agent.ResetPath();
            return;
        }

        switch (state)
        {
            case BotState.Chase:
                UpdateChase();
                break;

            case BotState.Attack:
                agent.ResetPath();
                break;

            case BotState.Custom:
                break;
        }
    }
    public void SetState(BotState newState)
    {
        state = newState;
    }
    protected virtual void UpdateChase()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            StartAttack();
            return;
        }

        animator.Play("Move");
        agent.SetDestination(player.position);
    }

    public virtual void StartAttack()
    {
        if (state == BotState.Attack) return;

        state = BotState.Attack;
        agent.ResetPath();
        G3_AudioManager.Instance.PlayGrannyCatchSound();
        Adventure_CameraSystem.Instance.ZoomToBot(true, camAttack);
        animator.Play("Attack");
        StartCoroutine(AttackRoutine());
    }

    protected virtual System.Collections.IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(1f);
        player.GetComponent<Parkour_DeathEvent>().Death();
    }
    public void Stop()
    {
        agent.ResetPath();
        canChase = false;
    }
    public virtual void ResetBot()
    {
        if (!gameObject.activeInHierarchy || (playerDeathEvent.m_data.CurrentCheckpointId != 1 &&
                                              playerDeathEvent.m_data.CurrentCheckpointId != 7 &&
                                              playerDeathEvent.m_data.CurrentCheckpointId != 13 &&
                                              playerDeathEvent.m_data.CurrentCheckpointId != 14 &&
                                              playerDeathEvent.m_data.CurrentCheckpointId != 22)) return;
        StopAllCoroutines();

        animator.Rebind();
        animator.Update(0);

        agent.enabled = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        state = BotState.Chase;

        canChase = initialCanChase;

        agent.enabled = true;
        if(playerDeathEvent.m_data.CurrentCheckpointId != 25)
        {
            Adventure_CameraSystem.Instance.ResetCamera(false);
        }
    }
}
