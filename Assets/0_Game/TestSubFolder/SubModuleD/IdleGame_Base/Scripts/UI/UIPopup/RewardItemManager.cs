
using System;
//using Firebase.Analytics;
using UnityEngine;

public class RewardItemManager: BasePopup, IInitable
{
    public int m_nextTime;
    public float m_currentTimeFloat;
    public float m_timeOffset;
    public int[] m_times;
    public RewardItemAddon[] m_items;

    public int m_trueInverseTime;
    private int m_delayedAccumulation;
    public RewardItemAddon m_itemAddonPrefab;
    public Transform m_contentTransform;

    public RewardItemData_SO m_rewardData;
    public MoneyController m_coin;
    public MoneyController m_gem;
    public MoneyController m_cup;
    public MoneyReceiveAnimator m_coinAnim;
    public MoneyReceiveAnimator m_gemAnim;
    public HatchEggPopup m_petPopup;

    public string m_rewardState;
    public int total = 12;
    private int m_available = 0;
    public RewardNoticeCounter m_counter;
    public override void Show()
    {
        base.Show();
        m_counter.StopCounter();
        OnReenable();
    }
    void TakeReward(int id)
    {
        RewardInfo rewardInfo = m_rewardData.m_rewards[id];
        total--;
        m_available--;
        AssignReward(rewardInfo);
        //TrackingEvent.LogFirebase($"climb_take_reward_{id}", null);
        /*TrackingEvent.LogFirebase($"take_reward_game_{GameManager.MiniGameIndex}", new Parameter[]
        {
            new Parameter(Consts.RewardID, id)
        });*/

        if (total == 0) ResetTotal();
        else
        {
            m_rewardState = IdleGameBase_PrefData.Replace(m_rewardState, id, 1, "2");
            IdleGameBase_PrefData.RewardState = m_rewardState;
        }
    }
    private bool m_canUpdate = true;
    void ResetTotal()
    {
        m_trueInverseTime = 3600;
      /*  total = 12;
        for (int i = 0; i < 12; i++) m_items[i].Reset();
        ResetItems();*/

        m_rewardState = new string('0', 50);
        IdleGameBase_PrefData.RewardState = m_rewardState;
        m_canUpdate = false;
        m_available = 0;
        m_currentLatest = -1;
    }
    public void AssignReward(RewardInfo info)
    {
        //ParticleManager.Instance.PlayConfetti();
        switch (info.m_infoType.m_type)
        {
            case RewardType.COIN:
                //m_coin.AddToTotalMoney(info.m_amount);
                ParticleManager.Instance.PlayConfetti();
                m_coinAnim.StartAnimate(info.m_amount);
                break;
            case RewardType.GEM:
                ParticleManager.Instance.PlayConfetti();
                m_gemAnim.StartAnimate(info.m_amount);
                break;
            case RewardType.PET:
                AudioManager.Instance.PlayCongrats();
                m_petPopup.ReceivePet(info.m_petId, this);
                break;
            case RewardType.CUP:
                ParticleManager.Instance.PlayConfetti();
                m_cup.AddToTotalMoney(info.m_amount);
                break;
        }
    }
    void ResetItems() {
        int acc = 3600 - m_trueInverseTime;
        int initSec = 150;
        int g = -acc;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                int id = i * 4 + j;
                if (id > m_times.Length) return;

                SetTimeOnItem(id, g);
                g += initSec;


            }
            initSec *= 2;
        }
    }
    public void Init()
    {
        total = 0;
        m_times = new int[12];
        m_items = new RewardItemAddon[12];
        m_rewardState = IdleGameBase_PrefData.RewardState;
        for (int i = 0; i < 12; i++)
        {
            RewardItemAddon rw = Instantiate(m_itemAddonPrefab, m_contentTransform);
            m_items[i] = rw;
            RewardInfo info = m_rewardData.m_rewards[i];
            rw.InitReward(info.m_infoType.m_type, info.m_amount, info.m_infoType.m_sprite, i, info.m_petId);
            rw.OnRewardClicked += TakeReward;
        }


        m_trueInverseTime = IdleGameBase_PrefData.RewardTime;
        
        for (int i = 0; i < 12; i++)
        {
            if (m_rewardState[i] == '2')
            {
                m_items[i].LockReceive();
                m_currentLatest = i + 1;
                continue;
            }
            if (m_rewardState[i] == '1')
            {
                m_items[i].AllowReceive();
                m_available++;
                m_currentLatest = i + 1;
            }
            total++;
        }
        if (total == 0) ResetTotal();
        ResetItems();
        SetNotice();
    }
    void SetNotice()
    {
        if (m_available == 0)
        {
            if (m_currentLatest < 0 || m_currentLatest >= 12) m_counter.SetRedNotice(false);
            else
            {
                m_counter.SetCounter(m_times[m_currentLatest]);
            }
        }
        else m_counter.SetRedNotice(true);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        IdleGameBase_PrefData.RewardTime = m_trueInverseTime;
        SetNotice();


    }
    private int m_currentLatest = -1;
    void SetTimeOnItem(int id, int itemTime)
    {
        if (m_items[id].m_received || m_items[id].m_canClick) return;
        m_times[id] = itemTime;
        if (m_times[id] <= 0)
        {
            m_times[id] = 0;
            m_items[id].AllowReceive();
            m_rewardState = IdleGameBase_PrefData.Replace(m_rewardState, id, 1, "1");
            IdleGameBase_PrefData.RewardState = m_rewardState;
            m_available++;
            m_currentLatest = id + 1;
        }
        else m_items[id].SetCountdown(itemTime);
    
    }
    public void OnReenable()
    {
        m_currentTimeFloat = Time.time;
        int nextTime = Mathf.CeilToInt(m_currentTimeFloat);
        if (nextTime == m_nextTime) return;
        m_delayedAccumulation = nextTime - m_nextTime;
        SetRewardItemsTime();
        m_nextTime = nextTime;
    }
    private void UpdateRewardItems()
    {
        for (int i = 0; i < m_times.Length; i++) SetTimeOnItem(i, m_times[i] - 1);
    }

    private void SetRewardItemsTime()
    {
        m_trueInverseTime -= m_delayedAccumulation;
        for (int i = 0; i < m_times.Length; i++) SetTimeOnItem(i, m_times[i] - m_delayedAccumulation);

    }
    private void Update()
    {
        if (!m_canUpdate) return;
        m_currentTimeFloat = Time.time;
        if (m_currentTimeFloat > m_nextTime)
        {
            m_nextTime++;
            m_trueInverseTime--;
            UpdateRewardItems();
        }
    }
}
