using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotState_Chase : Gr_BaseBotState
{
    private float chaseSpeed = 2.5f;
    public Gr_BotState_Chase(Gr_BotController bot, BotStateMachine stateMachine) : base(bot, stateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Entering Chase State");
        bot.Animator.Play("Run");
    }

    public override void Execute()
    {
        if (bot.BlackBoard.CanAttack)
        {
            stateMachine.ChangeState(bot.AttackState);
            return;
        }
        if (!bot.BlackBoard.CanSeePlayer)
        {
            stateMachine.ChangeState(bot.InvestigateState);
            return;
        }
        bot.Movement.MoveTo(bot.BlackBoard.PlayerTransform.position, chaseSpeed);
        bot.Movement.FacePlayer(bot.BlackBoard.PlayerTransform.position);
    }

    public override void Exit()
    {
    }
}
