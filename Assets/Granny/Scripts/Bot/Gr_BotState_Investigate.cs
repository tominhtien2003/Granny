using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotState_Investigate : Gr_BaseBotState
{
    private float investigateSpeed = 2.5f;

    private float maxSearchTime = 5f;
    private float searchTimer = 0f;
    private bool reachedLocation = false;
    public Gr_BotState_Investigate(Gr_BotController bot, BotStateMachine stateMachine) : base(bot, stateMachine)
    {
    }

    public override void Enter()
    {
        if (bot.BlackBoard.HasHeardSound)
        {
            bot.Movement.MoveTo(bot.BlackBoard.HeardPosition, investigateSpeed);
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

        if (!reachedLocation)
        {
            if (bot.Movement.HasReachedDestination(.2f))
            {
                reachedLocation = true;
                bot.Movement.StopMoving();  
            }
        }
        else
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= maxSearchTime)
            {
                stateMachine.ChangeState(bot.PatrolState);
            }
        }
    }

    public override void Exit()
    {
        bot.BlackBoard.ClearSoundData();
    }
}
