using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotState_Patrol : Gr_BaseBotState
{
    private float patrolSpeed = 2f;
    public Gr_BotState_Patrol(Gr_BotController bot, BotStateMachine stateMachine) : base(bot, stateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Entering Patrol State");
        bot.Animator.Play("Walk");
        if (bot.BlackBoard.CurrentWaypoint != null)
        {
            bot.Movement.MoveTo(bot.BlackBoard.CurrentWaypoint.transform.position, patrolSpeed);
        }
    }

    public override void Execute()
    {
        if (bot.BlackBoard.CanAttack)
        {
            stateMachine.ChangeState(bot.AttackState);
            return;
        }
        if (bot.BlackBoard.CanSeePlayer)
        {
            stateMachine.ChangeState(bot.ChaseState);
            return;
        }

        if (bot.BlackBoard.HasHeardSound)
        {
            stateMachine.ChangeState(bot.InvestigateState);
            return;
        }

        if (bot.Movement.HasReachedDestination(.2f))
        {
            var previousWaypoint = bot.BlackBoard.CurrentWaypoint;
            bot.BlackBoard.CurrentWaypoint = previousWaypoint.GetNeighbour();
            stateMachine.ChangeState(bot.IdleState);
        }
    }

    public override void Exit()
    {
        
    }
}
