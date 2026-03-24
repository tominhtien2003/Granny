using Cysharp.Text;
using KinematicCharacterController;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MoveAndPunchWall_Tutorial : TutorialScript
{
    public WallStats m_wall;
    public KinematicCharacterMotor m_player;

    public Transform m_punchButton;
    

    protected override void OnTutorialStart()
    {
        base.OnTutorialStart();
        TutorialGestures.Instance.SetLine(m_player.transform, m_wall.transform);
        m_wall.OnBroken += CheckBroken;
        m_pointing = false;
        MaxAmount = 5;
        CurrentAmount = 0;
    }

    void CheckBroken(int id)
    {
        CurrentAmount++;
        OnTextUpdate?.Invoke();
        if (id >= 4) ExitTutorial();
    }
    public override void ResetGraphic()
    {
        base.ResetGraphic();
        TutorialGestures.Instance.ResetAll();
    }
    private bool m_pointing = false;
    public override void OnTutorialUpdate(float dt)
    {
        base.OnTutorialUpdate(dt);
        float diff = Mathf.Abs(m_wall.transform.position.x - m_player.transform.position.x);
        if (diff <= 3)
        {
            if (!m_pointing)
            {
                TutorialGestures.Instance.SetTapAt(m_punchButton.transform);
                m_pointing = true;
            }
            
        }
        else if (m_pointing)
        {
            TutorialGestures.Instance.DisableTapAt();
            m_pointing = false;
        }
    }
    public override bool ConditionFulfilled()
    {
        return (PunchWall_GlobalStatusHolder.CurrentWall >= 4);
    }
}
