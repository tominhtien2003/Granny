using UnityEngine;
using UnityEngine.AI;

public class G3_BotChaseWithReturn : G3_BotChase
{
    [Header("Return Settings")]
    public float returnThreshold = 0.5f;

    public float returnSpeed = 0f;

    public float navMeshCheckDistance = 1f;

    private Vector3 spawnPosition;
    private bool isReturning = false;
    private int botNavMeshArea = -1;

    protected override void Start()
    {
        base.Start();
        spawnPosition = transform.position;

        if (returnSpeed <= 0)
            returnSpeed = agent.speed;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            botNavMeshArea = hit.mask;
        }
    }

    protected override void Update()
    {
        if (!canChase)
        {
            agent.ResetPath();
            return;
        }

        switch (state)
        {
            case BotState.Chase:
                UpdateChaseWithReturn();
                break;
            case BotState.Attack:
                agent.ResetPath();
                isReturning = false;
                break;
            case BotState.Custom:
                if (isReturning)
                    UpdateReturn();
                break;
        }
    }

    protected virtual void UpdateChaseWithReturn()
    {
        bool playerOnNavMesh = IsPlayerOnNavMesh();

        if (!playerOnNavMesh)
        {
            StartReturn();
            return;
        }

        if (isReturning)
        {
            isReturning = false;
            agent.speed = 3.5f;
            agent.stoppingDistance = attackRange;
        }

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= attackRange)
        {
            StartAttack();
            return;
        }

        animator.Play("Move");
        agent.SetDestination(player.position);
    }

    private bool IsPlayerOnNavMesh()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(player.position, out hit, navMeshCheckDistance, NavMesh.AllAreas))
        {
            if (botNavMeshArea == -1)
                return true;

            return (hit.mask & botNavMeshArea) != 0;
        }

        return false;
    }

    private void StartReturn()
    {
        if (isReturning) return;

        isReturning = true;
        state = BotState.Custom;
        agent.speed = returnSpeed;
        agent.stoppingDistance = returnThreshold;
        agent.SetDestination(spawnPosition);

        animator.Play("Move");
    }

    private void UpdateReturn()
    {
        float distToSpawn = Vector3.Distance(transform.position, spawnPosition);

        if (distToSpawn <= returnThreshold)
        {
            FinishReturn();
            return;
        }

        if (IsPlayerOnNavMesh())
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            if (distToPlayer <= attackRange || distToPlayer < distToSpawn)
            {
                isReturning = false;
                state = BotState.Chase;
                agent.speed = 3.5f;
                agent.stoppingDistance = attackRange;
            }
        }

        animator.Play("Move");
    }

    private void FinishReturn()
    {
        isReturning = false;
        state = BotState.Chase;
        agent.ResetPath();
        agent.speed = 3.5f;
        agent.stoppingDistance = attackRange;

        animator.Play("Idle");
    }

    public override void ResetBot()
    {
        isReturning = false;
        spawnPosition = transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            botNavMeshArea = hit.mask;
        }

        base.ResetBot();
    }
}