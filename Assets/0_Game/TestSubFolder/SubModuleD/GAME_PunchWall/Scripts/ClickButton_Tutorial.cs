using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton_Tutorial : TutorialScript
{
    public List<ButtonEffectLogic> m_correctButtons;
    public List<ButtonEffectLogic> m_wrongButtons;

    public override bool ConditionFulfilled()
    {
        return false;
    }
    protected override void OnTutorialStart()
    {
        base.OnTutorialStart();
        foreach (var button in m_correctButtons) button.onClick.AddListener(FinishTutor);
        foreach (var button in m_wrongButtons) button.onClick.AddListener(FailTutor);
        
        TutorialGestures.Instance.SetTapAt(m_correctButtons[0].transform);
    }

    void FailTutor()
    {
        RemoveEvents();
        ExitTutorial(false, 1);
    }
    void FinishTutor()
    {
        RemoveEvents();
        ExitTutorial();
    }
    public override void ResetGraphic()
    {
        base.ResetGraphic();
        TutorialGestures.Instance.DisableTapAt();

    }
    void RemoveEvents()
    {
        foreach (var button in m_correctButtons) button.onClick.RemoveListener(FinishTutor);
        foreach (var button in m_wrongButtons) button.onClick.RemoveListener(FailTutor);
    }
}
