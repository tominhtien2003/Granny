using UnityEngine;

public class Gr_BotState_Idle : Gr_BaseBotState
{
    private float idleTimer = 0;
    private float maxIdleTime = 2f;
    public Gr_BotState_Idle(Gr_BotController bot, BotStateMachine stateMachine) : base(bot, stateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Entering Idle State");
        bot.Animator.Play("Idle");
        bot.Movement.StopMoving();
        idleTimer = 0;
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
        idleTimer += Time.deltaTime;
        if (idleTimer >= maxIdleTime)
        {
            stateMachine.ChangeState(bot.PatrolState);
        }
    }

    public override void Exit()
    {
        
    }
}
