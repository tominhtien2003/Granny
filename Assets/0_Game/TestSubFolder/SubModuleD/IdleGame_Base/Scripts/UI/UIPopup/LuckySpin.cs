using Cysharp.Text;
using JetBrains.Annotations;
using LitMotion;
using LitMotion.Extensions;
using System.Collections.Generic;
//using Firebase.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum RewardType
{
    COIN = 0,
    GEM = 1,
    PET = 2,
    CUP = 3
}
public class LuckySpin : BasePopup
{
    public int m_slotAmounts;
    public RewardItemData_SO m_rewardData;
    public List<Image> m_rewards;
    public List<TMP_Text> m_amountDisplays;

    public RectTransform m_spinPanel;
    public MoneyReceiveAnimator m_anim;
    public RewardItemManager m_rewardPopup;

    public float m_textDistance;
    public float m_imageDistance;

    public ButtonEffectLogic m_spin;
    public ButtonEffectLogic m_freeSpin;
    public ButtonEffectLogic m_getMoreSpins;

    public int m_gemCost;
    public TMP_Text m_gemCostDisplay;

    public MoneyController m_gemController;
    public TMP_Text m_amountLeftText;
    public ArrowSpin m_arrow;

    private bool m_isSpinning = false;
    protected void Start()
    {
        InitRewards();
        m_amountLeftText.SetTextFormat("{0}", ClimbAndJump_DataController.SpinsLeft);
        m_gemCostDisplay.SetTextFormat("{0}", m_gemCost);
        m_spin.onClick.AddListener(AttemptSpin);
        m_freeSpin.onClick.AddListener(AdSpin);
        m_getMoreSpins.onClick.AddListener(GetMoreSpins);
    }
    void GetMoreSpins()
    {
        if (!m_gemController.AttemptPurchase(m_gemCost)) return;
        //TrackingEvent.LogFirebase("climb_buy_spin_with_gem", null);
        /*TrackingEvent.LogFirebase($"buy_spin_with_gem", new Parameter[]
        {
            new Parameter(Consts.GameID, GameManager.MiniGameIndex)
        });*/
        ClimbAndJump_DataController.SpinsLeft += 10;
        m_amountLeftText.SetTextFormat("{0}", ClimbAndJump_DataController.SpinsLeft);
    }
    public override void Hide()
    {
        if (m_isSpinning) return;
        else base.Hide();
    }
    void AdSpin()
    {
        if (m_isSpinning) return;
       /* AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, () =>
        {
            //TrackingEvent.LogFirebase("rw_climb_free_spin", null);
            TrackingEvent.LogFirebase("rw_free_spin", new Parameter[]
            {
                new Parameter(Consts.GameID, GameManager.MiniGameIndex)
            });*/
            StartSpin();
        //},Consts.NotiAdsFail, AdsAdapter.where.rw_climb_free_spin);
    }
    void InitRewards()
    {
        List<RewardInfo> rewards = m_rewardData.m_rewards;

        m_slotAmounts = m_rewards.Count;
        for (int i = 0; i < m_slotAmounts; i++)
        {
            RewardInfo info = rewards[i];
            if (info.m_infoType.m_type == RewardType.PET) 
                m_rewards[i].sprite = ClimbAndJump_DataController.Instance.m_petData.m_petItems[info.m_petId].m_displayIcon;
            else m_rewards[i].sprite = info.m_infoType.m_sprite;
            MoneyUIView.UpdateMoney(info.m_amount, m_amountDisplays[i]);
        }
    }
    [Button]
    public void ArrangeImages()
    {
        float angl = 360 / m_rewards.Count;
        for (int i = 0; i < m_rewards.Count; i++)
        {
            m_rewards[i].rectTransform.localPosition = Quaternion.Euler(0, 0, -angl * i) * Vector3.up * m_imageDistance;
            m_amountDisplays[i].rectTransform.localPosition = Quaternion.Euler(0, 0, -angl * i) * Vector3.up * m_textDistance;
            m_amountDisplays[i].rectTransform.localEulerAngles = Vector3.forward * -angl * i;
            m_rewards[i].rectTransform.localEulerAngles = Vector3.forward * -angl * i;
        }
    }
    [Button]
    public void TrySpin()
    {
        if (!Application.isPlaying) return;
        StartSpin();
    }
    private int m_selected = 0;
    void DeliverReward()
    {
        m_isSpinning = false;
        m_rewardPopup.AssignReward(m_rewardData.m_rewards[m_selected]);
        m_arrow.EndHit();
    }
    void AttemptSpin()
    {
        if (m_isSpinning) return;
        if (ClimbAndJump_DataController.SpinsLeft <= 0)
        {
            UINotify.Instance.Notify("NO SPIN LEFT!");
            return;
        }
        //TrackingEvent.LogFirebase("climb_lucky_spin", null);
       /* TrackingEvent.LogFirebase("lucky_spin", new Parameter[]
        {
            new Parameter(Consts.GameID, GameManager.MiniGameIndex)
        });*/
        ClimbAndJump_DataController.SpinsLeft--;
        m_amountLeftText.SetTextFormat("{0}", ClimbAndJump_DataController.SpinsLeft);
        StartSpin();
    }
    void StartSpin()
    {
        m_isSpinning = true;
        int rand = Random.Range(0,100);
        m_selected = 0;
        for (int i = 0; i < m_slotAmounts; i++)
        {
            int r = m_rewardData.m_rewards[i].m_chance;
            if (rand > r) rand -= r;
            else
            {
                m_selected = i;
                break;
            }
        }
        AudioManager.Instance.PlayLuckySpin();
        m_arrow.StartHit();

        float range = 360 / m_slotAmounts;
        float angl = m_selected * range;
        float finalAngl = angl;// Random.Range(angl - range * .5f, angl + range * .5f);
        int repeat = Random.Range(3, 5);
        float des = repeat * 360f + finalAngl;
        Debug.Log(angl + " " + finalAngl + " " + des + " " + repeat);
        LMotion.Create(m_spinPanel.localEulerAngles.z, des, Random.Range(4f, 5f))
       .WithEase(Ease.InOutCubic).WithOnComplete(DeliverReward).BindToLocalEulerAnglesZ(m_spinPanel).AddTo(m_spinPanel);
    }
}
