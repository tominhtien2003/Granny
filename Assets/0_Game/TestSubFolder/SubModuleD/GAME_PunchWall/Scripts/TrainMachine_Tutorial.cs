using System;

public class TrainMachine_Tutorial : TutorialScript
{
    public TrainingState m_trainState;
    public override bool ConditionFulfilled()
    {
        return PunchWall_DataController.CurrentAscension >= 1 || PunchWall_DataController.m_bodypartStrength[PunchWall_GlobalStatusHolder.CurrentBodypartId] >= 20;
    }

    protected override void OnTutorialStart()
    {
        base.OnTutorialStart();
        PunchWall_GlobalStatusHolder.OnBodypartReachedRequirement += FinishCondition;
        PunchWall_GlobalStatusHolder.OnTrain += UpdateAmount;
        m_trainState.OnStateChanged += ConditionFail;
        
        CurrentAmount = (int)PunchWall_DataController.m_bodypartStrength[PunchWall_GlobalStatusHolder.CurrentBodypartId];
        MaxAmount = 20;
    }

    private void UpdateAmount()
    {
        CurrentAmount = (int)PunchWall_DataController.m_bodypartStrength[PunchWall_GlobalStatusHolder.CurrentBodypartId];
        OnTextUpdate?.Invoke();
    }

    void ConditionFail(bool b)
    {
        if (!b)
        {
            ExitTutorial(false, 1);
        }
    }
    void FinishCondition(int a, int b)
    {
        if (a == PunchWall_GlobalStatusHolder.CurrentBodypartId) ExitTutorial();
    }

    protected override void ExitTutorial(bool isCompleted = true, int nextTutorial = 0)
    {
        PunchWall_GlobalStatusHolder.OnBodypartReachedRequirement -= FinishCondition;
        PunchWall_GlobalStatusHolder.OnTrain -= UpdateAmount;
        m_trainState.OnStateChanged -= ConditionFail;
        base.ExitTutorial(isCompleted, nextTutorial);
        
    }
}
