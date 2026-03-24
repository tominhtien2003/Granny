public class StaminaSlidePopup : BasePopup, IInitable
{
    public SlideClimb m_climbState;
    public ButtonEffectLogic m_acceptButton;

    public void Init()
    {
        m_climbState.OnTired += Show;
        m_acceptButton.onClick.AddListener(RewardStamina);
    }

    private void RewardStamina()
    {
        m_climbState.SetUnlimitedStamina();
        Hide();
       /* AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, () =>
        {
            TrackingEvent.LogFirebase($"rw_stamina_game", new Parameter[]
            {
                new Parameter(Consts.GameID, GameManager.MiniGameIndex)
            });
            m_climbState.SetUnlimitedStamina();
            Hide();
        }, () =>
        {
            Consts.NotiAdsFail();
        }, AdsAdapter.@where.rw_climb_stamina);*/
    }


}
