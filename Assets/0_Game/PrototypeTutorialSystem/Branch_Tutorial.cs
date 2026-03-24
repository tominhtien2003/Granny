using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch_Tutorial : TutorialScript
{
    public BaseCustomCondition m_condition;

    public override bool ConditionFulfilled()
    {
        return m_condition.Condition();
    }

    protected override void OnTutorialStart()
    {
        base.OnTutorialStart();
        ExitTutorial(true, 1);
    }
}
