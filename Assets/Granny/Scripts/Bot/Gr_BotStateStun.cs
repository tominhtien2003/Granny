using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotStateStun : Gr_BaseBotState
{
    public Gr_BotStateStun(Gr_BotController bot, BotStateMachine stateMachine) : base(bot, stateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Entering Stun State");
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
