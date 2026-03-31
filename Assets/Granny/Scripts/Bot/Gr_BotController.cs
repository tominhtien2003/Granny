using System;
using UnityEngine;
using UnityEngine.AI;

public class Gr_BotController : MonoBehaviour
{
    public Waypoint StartWaypoint;
    [SerializeField] Transform PlayerTransform;
    public Animator Animator;
    private BotStateMachine stateMachine;
    public Gr_BotBlackBoard BlackBoard { get;private  set; }
    
    public Gr_BotState_Idle IdleState { get;private  set; }
    public Gr_BotState_Investigate InvestigateState { get;private  set; }
    public Gr_BotState_Chase ChaseState { get;private  set; }
    public Gr_BotState_Patrol PatrolState { get;private  set; }
    public Gr_BotStateAttack AttackState { get;private  set; }
    public Gr_BotStateStun StunState { get;private  set; }
    
    public Gr_BotVision Vision{get;private  set; }
    public Gr_BotMovement Movement { get;private  set; }
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    private void Awake()
    {
        stateMachine = new BotStateMachine();
        BlackBoard = new Gr_BotBlackBoard();
        BlackBoard.PlayerTransform = PlayerTransform;
        
        
        Vision = GetComponent<Gr_BotVision>();
        Movement = GetComponent<Gr_BotMovement>();
        agent = GetComponent<NavMeshAgent>();

        IdleState = new Gr_BotState_Idle(this, stateMachine);
        InvestigateState = new Gr_BotState_Investigate(this,stateMachine);
        ChaseState = new Gr_BotState_Chase(this,stateMachine);
        PatrolState = new Gr_BotState_Patrol(this,stateMachine);
        AttackState = new Gr_BotStateAttack(this,stateMachine);
        StunState = new Gr_BotStateStun(this, stateMachine);
    }

    private void Start()
    {
        BlackBoard.CurrentWaypoint = StartWaypoint;
        stateMachine.ChangeState(PatrolState);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
