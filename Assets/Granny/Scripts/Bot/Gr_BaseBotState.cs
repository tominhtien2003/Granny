using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gr_BaseBotState : Gr_IState
{
    protected Gr_BotController bot;
    protected BotStateMachine stateMachine;

    protected Gr_BaseBotState(Gr_BotController bot, BotStateMachine stateMachine)
    {
        this.bot = bot;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        
    }

    public virtual void Execute()
    {
        
    }

    public virtual void Exit()
    {
        
    }
}
