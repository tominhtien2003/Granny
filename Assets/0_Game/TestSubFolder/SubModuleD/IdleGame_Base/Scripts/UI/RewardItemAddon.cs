using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardItemAddon : MonoBehaviour
{
    public RewardType m_type;
    public Image m_rewardImage;
    public TMP_Text m_amountText;
    public TMP_Text m_countdownText;
    public long m_amount;
    public ButtonEffectLogic m_button;
    public UnityAction<int> OnRewardClicked;
    public bool m_canClick = false;
    public bool m_received = false;
    public int m_id;
    public Image m_tickImage;
    public void InitReward(RewardType rType, long amount, Sprite typeSprite, int id, int petId)
    {
        m_id = id;
        m_amount = amount;
        m_type = rType;

        if (rType == RewardType.PET) m_rewardImage.sprite = ClimbAndJump_DataController.Instance.m_petData.m_petItems[petId].m_displayIcon;
        else m_rewardImage.sprite = typeSprite;

        m_amountText.SetTextFormat("{0}", m_amount);
        m_button.onClick.AddListener(() =>
        {
            if (m_canClick && !m_received)
            {
                LockReceive();
                OnRewardClicked(m_id);
            }
        });

    }

    public void SetCountdown(int seconds)
    {
        int minutes = seconds / 60;
        int rem = seconds % 60;
        m_countdownText.SetTextFormat("{0:D2}:{1:D2}", minutes, rem);

    }
    public void AllowReceive()
    {
        m_canClick = true;
        m_countdownText.SetText("Claim!");
    }

    public void Reset()
    {
        m_canClick = false;
        m_received = false;
        m_tickImage.enabled = false;
    }
    public void LockReceive()
    {
        m_tickImage.enabled = true;
        m_received = true;
        m_countdownText.SetText("Claimed!");
    }
}
