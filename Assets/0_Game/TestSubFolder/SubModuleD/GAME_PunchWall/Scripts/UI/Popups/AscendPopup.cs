using Cysharp.Text;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AscendPopup : BasePopup, IInitable
{
    public List<Image> m_bodypartFills;
    public TMP_Text[] m_bodypartTxts;
    public PunchWallScript m_punchWallScript;
    public TMP_Text m_currentLevel;
    public TMP_Text m_nextLevel;
    public TMP_Text m_currentMulti;
    public TMP_Text m_nextMulti;
    public SkinShopListDisplayPopup m_skinShop;

    private Utf16ValueStringBuilder m_sb;
    public ButtonEffectLogic m_ascendButton;
    public ButtonEffectLogic m_skipButton;
    private List<double> m_vals;
    private List<double> m_multis;
    private List<int> m_skinsId;

    private bool m_maxed = false;
    public GameObject m_unMaxedInfo;
    public GameObject m_maxedInfo;

    public Image m_previewSkinImage;
    public TMP_Text m_skinName;

    public SkinController BloxSkinController;
    public void Init()
    {
        BloxSkinController = ClimbAndJump_DataController.BloxSkinController;
        m_sb = ZString.CreateStringBuilder();
        m_vals = PunchWall_DataController.Instance.m_ascendRequirementData.m_values;
        m_multis = PunchWall_DataController.Instance.m_ascendMultiplierData.m_values;
        m_skinsId = PunchWall_DataController.Instance.m_ascendUnlockData.m_values;
        m_ascendButton.onClick.AddListener(() =>
        {
            if (PunchWall_GlobalStatusHolder.m_cnt >= 3 && !m_maxed)
            {
                Ascend();
            }
        });
        m_skipButton.onClick.AddListener(() =>
        {
            if (!m_maxed)
            {
                Ascend();
                /*TrackingEvent.LogFirebase($"rw_skip_ascend", new Parameter[]
                {
                    new Parameter(Consts.ascendLevel, PunchWall_DataController.CurrentAscension)
                });

                AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, () =>
                {
                    Ascend();
                },
                Consts.NotiAdsFail, AdsAdapter.@where.rw_skip_ascend);*/
            }
        });
    }
    public override void Show()
    {
        base.Show();
        UpdateRequiredPower();
        if (PunchWall_GlobalStatusHolder.m_cnt >= 3 && !m_maxed) m_skipButton.gameObject.SetActive(false);
        else m_skipButton.gameObject.SetActive(true);
    }
    void AppendNum(double p1)
    {
        if (p1 < 1000)
        {
            m_sb.AppendFormat("{0}", p1);
        }
        else
        {
            double d1 = Utils.GetNumberAroundString(p1, out int suffix1);
            if (Utils.HasOneAfterDecimal(d1)) m_sb.AppendFormat("{0:F1}{1}", d1, Utils.suffixes[suffix1]);
            else m_sb.AppendFormat("{0}{1}", d1, Utils.suffixes[suffix1]);
        }
    }
    public void UpdateRequiredPower()
    {
        if (!m_maxed && PunchWall_DataController.CurrentAscension == m_vals.Count - 1)
        {
            m_maxedInfo.SetActive(true);
            m_unMaxedInfo.SetActive(false);
            m_maxed = true;
        }
        if (m_maxed) return;
        for (int i = 0; i < 3; i++)
        {
            double p1 = PunchWall_DataController.m_bodypartStrength[i];
            double p2 = m_vals[PunchWall_DataController.CurrentAscension];
            float ratio = (float)(p1 / p2);
            m_bodypartFills[i].fillAmount = ratio;
            
            m_sb.Clear();
            AppendNum(p1);
            m_sb.Append("/");
            AppendNum(p2);
            m_bodypartTxts[i].SetText(m_sb);
        }
        m_currentLevel.SetTextFormat("{0}",PunchWall_DataController.CurrentAscension);
        m_nextLevel.SetTextFormat("{0}",PunchWall_DataController.CurrentAscension + 1);
        m_currentMulti.SetTextFormat("{0}",m_multis[PunchWall_DataController.CurrentAscension]);
        m_nextMulti.SetTextFormat("{0}",m_multis[PunchWall_DataController.CurrentAscension + 1]);
        m_previewSkinImage.sprite = ClimbAndJump_DataController.Instance.m_skinData.m_shopItemData[m_skinsId[PunchWall_DataController.CurrentAscension]].m_displayIcon;
        m_skinName.SetText(ClimbAndJump_DataController.Instance.m_skinData.m_shopItemData[m_skinsId[PunchWall_DataController.CurrentAscension]].m_itemName);
    }
    void Ascend()
    {
        PunchWall_DataController.CurrentAscension++;
       /* TrackingEvent.LogFirebase($"ascend_level_up", new Parameter[]
        {
            new Parameter(Consts.ascendLevel, PunchWall_DataController.CurrentAscension)
        });*/
        

        if (PunchWall_DataController.CurrentAscension >= m_vals.Count)
        {
            this.LogError("this isn't supposed to happen");
            return;
        }
        PunchWall_GlobalStatusHolder.Instance.ResetRequirement();
        m_punchWallScript.PlayAscendEffect();
        m_punchWallScript.UpdateBodyPartValues();

        BloxSkinController.m_skinDataHolder.AutoFillSkin(m_skinsId[PunchWall_DataController.CurrentAscension - 1]);
        Hide();
    }

    public override void Hide()
    {
        base.Hide();
        //AdsAdapter.Instance.ShowInterstitial(GameManager.MiniGameIndex, AdsAdapter.@where.inter_ascend_popup);
    }


    void OnDestroy()
    {
        m_sb.Dispose();
    }
}
