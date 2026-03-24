using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAscendPopup_Tutorial : TutorialScript
{
    public ButtonEffectLogic m_ascendPopupButton;

    protected override void OnTutorialStart()
    {
        base.OnTutorialStart();
        m_ascendPopupButton.onClick.AddListener(ExitTutor);
        TutorialGestures.Instance.SetTapAt(m_ascendPopupButton.transform);
    }
    void ExitTutor()
    {
        ExitTutorial();
    }
    protected override void ExitTutorial(bool isCompleted = true, int nextTutorial = 0)
    {
        base.ExitTutorial(isCompleted, nextTutorial);
        m_ascendPopupButton.onClick.RemoveListener(ExitTutor);
    }
    public override void ResetGraphic()
    {
        base.ResetGraphic();
        TutorialGestures.Instance.DisableTapAt();
    }

    public override bool ConditionFulfilled()
    {
        return PunchWall_DataController.CurrentAscension >= 1;
    }
}
