using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoToMachine_Tutorial : TutorialScript
{
    public MachinesManager m_machinesManager;
    public TrainingState m_player;
    protected override void OnTutorialStart()
    {
        base.OnTutorialStart();
        TutorialGestures.Instance.SetLine(m_player.transform, m_machinesManager.m_unOccupiedMachines[0].transform);
        m_player.OnStateChanged += CheckState;
    }
    void CheckState(bool state)
    {
        if (state)
        {
            ExitTutorial();
        }
    }
    public override void ResetGraphic()
    {
        base.ResetGraphic();
        TutorialGestures.Instance.DisableLine();

    }
    protected override void ExitTutorial(bool isCompleted = true, int nextTutorial = 0)
    {
        
        base.ExitTutorial(isCompleted, nextTutorial);
        m_player.OnStateChanged -= CheckState;
    }
    public override bool ConditionFulfilled()
    {
        return false;
    }
}
