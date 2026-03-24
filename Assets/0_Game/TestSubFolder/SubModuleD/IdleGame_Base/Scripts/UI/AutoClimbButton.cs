using UnityEngine;
using UnityEngine.UI;
public class AutoClimbButton : MonoBehaviour
{
    public ButtonEffectLogic m_autoClimbButton;

    public Image m_hasNotClimbed;
    public Image m_climbed;
    public Image m_adIcon;

    public IStateChangeEvents m_state;
    public IAutoClimb m_autoState;
    private bool m_autoClimbState = false;
    private void Start()
    {
        m_state.OnStateChanged += OnStateChanged;
        m_autoClimbButton.onClick.AddListener(WatchAdAndActivate);
    }
    void OnStateChanged(bool state)
    {
        m_autoClimbButton.gameObject.SetActive(state);
        SetAutoClimbState(false);
    }

    void WatchAdAndActivate()
    {
        /*AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, () =>
        {
            TrackingEvent.LogFirebase("rw_climb_auto", new Parameter[]
            {
                new Parameter(Consts.GameID, GameManager.MiniGameIndex)
            });
            */
            m_adIcon.enabled = false;
            SwitchAutoClimbState();
            m_autoClimbButton.onClick.RemoveAllListeners();
            m_autoClimbButton.onClick.AddListener(SwitchAutoClimbState);
       /* }, () =>
        {
            Consts.NotiAdsFail();
        }, AdsAdapter.@where.rw_climb_auto_climb);*/
    }
    void SwitchAutoClimbState()
    {
        SetAutoClimbState(!m_autoClimbState);
    }
    void SetAutoClimbState(bool state)
    {
        m_autoClimbState = state;
        m_autoState.SetAuto(m_autoClimbState);
        m_hasNotClimbed.enabled = !state;
        m_climbed.enabled = state;
    }


}
