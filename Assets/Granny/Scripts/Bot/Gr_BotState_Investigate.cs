using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotState_Investigate : Gr_BaseBotState
{
    private float investigateSpeed = 2.5f;

    // float maxSearchTime = 5f;
    //private float searchTimer = 0f;
    //private bool reachedLocation = false;
    
    private Vector3 targetLocation;
    public Gr_BotState_Investigate(Gr_BotController bot, BotStateMachine stateMachine) : base(bot, stateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Entering Investigate State");
        if (bot.BlackBoard.HasHeardSound)
        {
            bot.Animator.Play("Walk");
            targetLocation = bot.BlackBoard.HeardPosition;
            //bot.Movement.MoveTo(bot.BlackBoard.HeardPosition, investigateSpeed);
        }
        else if (bot.BlackBoard.HasLastKnownPlayerPosition)
        {
            bot.Animator.Play("Run");
            targetLocation = bot.BlackBoard.LastKnownPlayerPosition;
            //bot.Movement.MoveTo(bot.BlackBoard.LastKnownPlayerPosition, investigateSpeed);
        }
        bot.Movement.MoveTo(targetLocation, investigateSpeed);
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

        if (bot.BlackBoard.HeardPosition != targetLocation)
        {
            bot.Movement.MoveTo(targetLocation, investigateSpeed);
        }
        if (bot.Movement.HasReachedDestination(.2f))
        {
            //reachedLocation = true;
            bot.Movement.StopMoving();  
            stateMachine.ChangeState(bot.IdleState);
        }
        //if (!reachedLocation)
        //{
            //if (bot.Movement.HasReachedDestination(.2f))
            //{
                //reachedLocation = true;
                //bot.Movement.StopMoving();  
            //}
        //}
        //else
        //{
            //searchTimer += Time.deltaTime;
            //if (searchTimer >= maxSearchTime)
            //{
                //stateMachine.ChangeState(bot.IdleState);
            //}
        //}
    }

    public override void Exit()
    {
        //searchTimer = 0f;
        bot.BlackBoard.ClearSoundData();
        bot.BlackBoard.ClearLastKnownPosition();
    }
}
