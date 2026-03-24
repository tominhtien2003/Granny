using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
#if FIREBASE_AVAILABLE
using Firebase.Analytics;
#endif

public class FreePopup : BasePopup
{
    public List<RewardInfo> m_availableRewards;
    public ButtonEffectLogic m_acceptButton;
    public TMP_Text m_amountText;
    public TMP_Text m_titleText;
    public Image m_typeImage;
    public Image m_iconGems;
    public Image m_iconCoins;
    public string m_namePopup = "";

    public MoneyReceiveAnimator m_animator;
    public RewardItemManager m_rewardManager;

    public MoneyController m_coinController;
    public int m_currentId = -1;


    protected override void Awake()
    {
        base.Awake();

        if (Utils_UIController.on_off_reward_button)
        {
            if (m_rewardManager)
            {
                m_acceptButton.onClick.AddListener(Reward);
            }
            else
            {
                m_acceptButton.onClick.AddListener(RewardCoin);
            }
        }
        else
        {
            m_acceptButton.gameObject.SetActive(false);
        }
        
      
    }
    public override void InitialiseUI()
    {
        base.InitialiseUI();
        if (m_rewardManager)
        {
    
            m_coinController = m_rewardManager.m_coin;
        }
        else
        {
          
            m_coinController = m_animator.m_controller;
        }
    }
    void RewardCoin()
    {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase($"rw_free_{(m_availableRewards[m_currentId].m_infoType.m_type == RewardType.COIN ? Consts.coins : Consts.gems)}", new Parameter[]
            {
                new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
            });
#endif
            ParticleManager.Instance.PlayConfetti();
            m_animator.StartAnimate(m_currentCoin);
            Hide();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_free);
#elif NO_ADS
        ParticleManager.Instance.PlayConfetti();
        m_animator.StartAnimate(m_currentCoin);
        Hide();
#else
    Consts.NotiNoAds();
#endif

    }
    private void Reward()
    {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase($"rw_free_{(m_availableRewards[m_currentId].m_infoType.m_type == RewardType.COIN ? Consts.coins : Consts.gems)}", new Parameter[]
            {
                new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
            });
#endif
            m_rewardManager.AssignReward(m_availableRewards[m_currentId]);
            Hide();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_free);
#elif NO_ADS
        m_rewardManager.AssignReward(m_availableRewards[m_currentId]);
        Hide();
#else
    Consts.NotiNoAds();
#endif
    }
    private double m_currentCoin;
    public void SetupOpen(int id)
    {
        if (m_currentId == id) return;
        m_currentId = id;
        double rewardCoin = m_availableRewards[id].m_amount;
        if (m_typeImage)
        {
            m_typeImage.sprite = m_availableRewards[id].m_infoType.m_sprite;
        }
        bool isCoin = (m_availableRewards[id].m_infoType.m_type == RewardType.COIN);
        if (m_namePopup.Equals(""))
        {
            m_titleText.SetText(isCoin ? "COINS" : "GEMS");
        }
        else
        {
            m_titleText.SetText(m_namePopup);
        }
        DataController.MaxCoin = m_coinController.m_totalMoney;
        if (isCoin && DataController.MaxCoin > 1000000)
        {
            
            rewardCoin = DataController.MaxCoin * .1f;
        }
        MoneyUIView.UpdateMoneyFormat(rewardCoin, m_amountText, true, "+{0:F1}{1}");
        m_currentCoin = rewardCoin;
        m_iconGems.enabled = !isCoin;
        m_iconCoins.enabled = isCoin;
    }


    public override void Show()
    {
        base.Show();
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowMrec();
#endif
    }

    public override void Hide()
    {
        base.Hide();
#if ADS_AVAILABLE
        AdsAdapter.Instance.HideMrec();
        AdsAdapter.Instance.ShowInterstitial(Mediation_Manager.GameID(), AdsAdapter.@where.inter_close_popup);
#endif
    }
}
