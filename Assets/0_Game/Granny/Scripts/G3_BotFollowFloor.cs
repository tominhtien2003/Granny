using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum FloorLevel { Floor1, Floor2 }
public enum BotState
{
    Chase,
    Continue,
    Riding,
    Angry,
    Attack,
    Custom
}

public class G3_BotFollowFloor : G3_BotChase
{
    [Header("References")]
    public G3_ChairElevator elevator;

    [Header("Elevator Points")]
    public Transform seatPoint_Floor1;
    public Transform seatPoint_Floor2;
    public Transform exitPoint_Floor1;
    public Transform exitPoint_Floor2;

    [Header("Bot Logic")]
    public FloorLevel botFloor = FloorLevel.Floor1;
    public FloorLevel playerFloor = FloorLevel.Floor1;

    private FloorLevel initialFloor;

    protected override void Start()
    {
        base.Start();

        initialFloor = botFloor;
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
                UpdateChase();
                break;
            case BotState.Riding:
                animator.Play("Sit");
                break;

            case BotState.Angry:
            case BotState.Attack:
                agent.ResetPath(); 
                break;
        }
    }
    private bool hasTriggeredAngry = false;
    protected override void UpdateChase()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!hasTriggeredAngry && playerDeathEvent.m_data.CurrentCheckpointId == 2)
        {
            StartAngry();
            hasTriggeredAngry = true;
            return;
        }

        if (botFloor != playerFloor)
        {
            Vector3 target = (playerFloor == FloorLevel.Floor2)
                ? seatPoint_Floor1.position
                : seatPoint_Floor2.position;
            animator.Play("Move");
            agent.SetDestination(target);
            return;
        }

        if (dist <= attackRange)
        {
            StartAttack();
            return;
        }
        animator.Play("Move");
        agent.SetDestination(player.position);
    }

    public void StartAngry()
    {
        if (state == BotState.Angry) return;

        state = BotState.Angry;

        agent.ResetPath();
        animator.Play("Angry");
        StartCoroutine(AngryRoutine());
    }

    IEnumerator AngryRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        state = BotState.Chase;
    }


    public override void StartAttack()
    {
        if (state == BotState.Attack) return;
        hasTriggeredAngry = false;
        state = BotState.Attack;
        agent.ResetPath();
        G3_UIManager.Instance.uiDeep.SetActive(false);
        animator.Play("Attack");

        Adventure_CameraSystem.Instance.ZoomToBot(true, camAttack);

        StartCoroutine(AttackRoutine());
    }

    protected override IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(1f);

        player.GetComponent<Parkour_DeathEvent>().Death();

    }

    public void SetBotFloor(FloorLevel zoneFloor)
    {
        botFloor = zoneFloor;
        CallElevator(zoneFloor);
    }

    public void SetPlayerFloor(FloorLevel zoneFloor)
    {
        playerFloor = zoneFloor;
    }

    public void CallElevator(FloorLevel targetFloor)
    {
        if (state != BotState.Chase || botFloor == playerFloor) return;

        state = BotState.Riding;
        agent.ResetPath();

        StartCoroutine(RideElevator(targetFloor));
    }

    IEnumerator RideElevator(FloorLevel goTo)
    {
        Transform seat = (goTo == FloorLevel.Floor2) ? seatPoint_Floor1 : seatPoint_Floor2;

        agent.enabled = false;
        transform.SetParent(elevator.transform);
        transform.position = seat.position;
        transform.rotation = seat.rotation;

        elevator.MoveTo(goTo);

        yield return new WaitUntil(() => elevator.isDone);

        agent.enabled = true;
        transform.SetParent(parent);

        Transform exitPoint = (goTo == FloorLevel.Floor2) ? exitPoint_Floor2 : exitPoint_Floor1;
        transform.position = exitPoint.position;

        //botFloor = goTo;
        state = BotState.Chase;
    }
    public void EnableChase(bool enable)
    {
        canChase = enable;
    }
    public override void ResetBot()
    {
        base.ResetBot();
        transform.SetParent(parent);
        botFloor = initialFloor;
        playerFloor = initialFloor;
        elevator.ResetElevator();
    }
}
