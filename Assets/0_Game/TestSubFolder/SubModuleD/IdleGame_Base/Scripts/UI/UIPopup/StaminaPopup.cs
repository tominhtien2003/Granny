using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
//using Firebase.Analytics;
using UnityEngine;

public class StaminaPopup : BasePopup, IInitable
{
    
    public IAutoClimb m_climbState;
    public ButtonEffectLogic m_acceptButton;
    public  void Init()
    {
        m_climbState = FindObjectOfType<KinematicCharacterMotor>().GetComponentInChildren<IAutoClimb>();
        m_climbState.OnTired += Show;
        m_acceptButton.onClick.AddListener(RewardStamina);
    }

    private void RewardStamina()
    {
        /*AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, () =>
        {
            //TrackingEvent.LogFirebase($"rw_climb_stamina", null);
            TrackingEvent.LogFirebase($"rw_stamina_game", new Parameter[]
            {
                new Parameter(Consts.GameID, GameManager.MiniGameIndex)
            });*/
            m_climbState.SetUnlimitedStamina();
            Hide();
       /* }, () =>
        {
            Consts.NotiAdsFail();
        }, AdsAdapter.@where.rw_climb_stamina);*/
    }
    
    public override void Hide()
    {
        base.Hide();
        /*AdsAdapter.Instance.HideMrec();
        AdsAdapter.Instance.ShowInterstitial(GameManager.MiniGameIndex, AdsAdapter.@where.inter_stamina_popup);*/
    }
    public override void Show()
    {
        base.Show();
        //AdsAdapter.Instance.ShowMrec();
    }



}
