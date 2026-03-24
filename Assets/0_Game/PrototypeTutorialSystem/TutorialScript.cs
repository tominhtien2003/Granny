using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialScript : MonoBehaviour
{
    public int m_tutorialId;

    public bool IsCheckpoint = false;
    public List<TutorialScript> NextTutorials;

    public Action<int> OnTutorialDone;

    public bool HasPassed;

    [Header ("Tutorial text")]
    public bool HasText = true;
    public Action OnTextUpdate;
    public string TutorialDetails;

    public int MaxAmount;
    public int CurrentAmount;
    public abstract bool ConditionFulfilled();
   
    public void StartTutorial()
    {
        if (Tutorial_DataController.CurrentTutorialStep > m_tutorialId || ConditionFulfilled())
        {
            CompleteThisTutorialStep();
            return;
        }
        if (IsCheckpoint) Tutorial_DataController.CurrentTutorialStep = m_tutorialId;
        
        OnTutorialStart();
        if (HasText && !HasPassed) OnTextUpdate?.Invoke();
    }

    protected virtual void OnTutorialStart()
    {

    }
    public virtual void OnTutorialUpdate(float dt)
    {
        
    }
    public virtual void ResetGraphic()
    {

    }
    protected void CompleteThisTutorialStep(bool isCompleted = true, int nextTutorial = 0)
    {
        HasPassed = isCompleted;
        if (HasPassed) OnTextUpdate = null;
        enabled = false;
        ResetGraphic();
        OnTutorialDone?.Invoke(nextTutorial);
    }
    protected virtual void ExitTutorial(bool isCompleted = true, int nextTutorial = 0)
    {
        CompleteThisTutorialStep(isCompleted, nextTutorial);
    }

}
