using Cysharp.Text;
using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Prototype_TutorialManager : MonoBehaviour
{
    public TutorialScript m_currentTutorial;

    public Transform m_tutorialTab;
    public TMP_Text m_tutorialText;
    private void Awake()
    {
        m_currentTutorial = null;
        
    }

    private void UpdateText()
    {
        if (m_currentTutorial.MaxAmount <= 0) m_tutorialText.SetText(m_currentTutorial.TutorialDetails);
        else m_tutorialText.SetTextFormat(m_currentTutorial.TutorialDetails, m_currentTutorial.CurrentAmount, m_currentTutorial.MaxAmount);
    }

    private void Start()
    {
        TutorialScript[] scripts = GetComponentsInChildren<TutorialScript>(true);
        int cnt = -1;
        int currentStep = Tutorial_DataController.CurrentTutorialStep;

        if (currentStep == -1)
        {
            EndTutorials();
            return;
        }
        for (int i = 0; i < scripts.Length; i++)
        {
            var v = scripts[i];
            if (v.IsCheckpoint) cnt++;
            v.m_tutorialId = cnt;
            v.enabled = false;
            v.OnTutorialDone += OnTutorialDone;
            if (v.IsCheckpoint && v.m_tutorialId == currentStep) m_currentTutorial = v;
        }
        RunCurrentTutorial();
    }
    private void Update()
    {
        m_currentTutorial.OnTutorialUpdate(Time.deltaTime);
    }
    void OnTutorialDone(int i)
    {
        if (m_currentTutorial.NextTutorials.Count >= i + 1 && m_currentTutorial.NextTutorials[i] != null)
        {
            m_currentTutorial = m_currentTutorial.NextTutorials[i];
            RunCurrentTutorial();
        }
        else EndTutorials();
    }

    void EndTutorials()
    {
        gameObject.SetActive(false);
        m_tutorialTab.gameObject.SetActive(false);
        Tutorial_DataController.CurrentTutorialStep = -1;
    }
    void RunCurrentTutorial()
    {
        if (m_currentTutorial == null) EndTutorials();
        if (m_currentTutorial.HasText)
        {
            m_tutorialTab.localScale = Vector3.zero;
            Tween.Scale(m_tutorialTab,1, .2f, Ease.OutBack);

            m_currentTutorial.OnTextUpdate += UpdateText;
        }
        m_currentTutorial.StartTutorial();
    }

}
