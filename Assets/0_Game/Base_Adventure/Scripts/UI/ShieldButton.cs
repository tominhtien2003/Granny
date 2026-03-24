using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
#if FIREBASE_AVAILABLE
using Firebase.Analytics;
#endif

public class ShieldButton : MonoBehaviour
{
    Tween _shieldTween;
    [SerializeField] private Image shieldFiller;
    [SerializeField] private GameObject shieldAdIcon;
    public ButtonEffectLogic shieldButton;
    //public ShieldComponent m_shield;
    
    private int _timeShield = 60;
    private void Awake()
    {
        if (Utils_UIController.on_off_reward_button)
        {
            shieldButton.onClick.AddListener(ShieldClick);
        }
        else
        {
            shieldButton.gameObject.SetActive(false);
        }
        
    }
    private void ShieldClick()
    {
        if (ShieldComponent.isShield)
        {
            return;
        }

 #if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
            Reward_Shield();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_shield);
#elif NO_ADS    
            Reward_Shield();
#else
    Consts.NotiNoAds();
#endif
        
        
    }
    public void Temporal_Shield()
    {
        StartShield(3);
    }

    void StartShield(float time)
    {
        shieldFiller.fillAmount = 1f;
        SetShieldState(true);
        _shieldTween = Tween.UIFillAmount(shieldFiller, 1f, 0f, time)

            /*  Tween.Custom(target: this, startValue: 1f, endValue: 0f, duration: Lucky_Const.ShieldDuration,
                  onValueChange: (t, v) =>
                  {
                      t.shieldFiller.fillAmount = v;
                  })*/
            .OnComplete(target: this, t =>
            {
                t.shieldFiller.fillAmount = 0f;
                SetShieldState(false);
            }, false);
    }
    private void Reward_Shield()
    {
#if FIREBASE_AVAILABLE
        TrackingEvent.LogFirebase(AdsAdapter.@where.rw_shield.ToString(), new Parameter[]
        {
            new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
        });
#endif         
        StartShield(_timeShield);
    }

    private void SetShieldState(bool state)
    {
        ShieldComponent.Instance.ToggleEffect(state);
        //m_shield.ToggleEffect(state);
        shieldAdIcon.SetActive(!state);
        ShieldComponent.isShield = state;
    }
}
