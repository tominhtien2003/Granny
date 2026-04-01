using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotStateAttack : Gr_BaseBotState
{
    public Gr_BotStateAttack(Gr_BotController bot, BotStateMachine stateMachine) : base(bot, stateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Entering Attack State");
        bot.Movement.StopMoving();
        bot.Animator.Play("Angry");
        bot.botAttackCam.Priority = 99;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
