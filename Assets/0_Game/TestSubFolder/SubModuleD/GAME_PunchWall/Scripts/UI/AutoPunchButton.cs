using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPunchButton : MonoBehaviour
{
    private ButtonGraphicToggler m_toggler;
    public PunchWall_StateMachineInitializer m_stateInit;
    private bool m_currentState = false;
    private void Awake()
    {
        GetComponentInChildren<ButtonEffectLogic>(true).onClick.AddListener(ToggleState);
        m_toggler = GetComponent<ButtonGraphicToggler>();
        SetState(false);
    }

    void SetState(bool state)
    {
        m_currentState = state;
        m_toggler.Toggle(state);
        m_stateInit.m_punchWallMovement.AutoPunch = state;
    }
    void ToggleState()
    {
        if (m_currentState)
        {
            SetState(false);
        }
        else
        {
            SetState(true);
            /*TrackingEvent.LogFirebase("rw_auto_punch", new Parameter[]
            {
                new Parameter(Consts.GameID, GameManager.MiniGameIndex)
            });
            AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, () => SetState(true),
                Consts.NotiAdsFail, AdsAdapter.@where.rw_punch_wall_auto_punch);*/
        }
        

    }
}
