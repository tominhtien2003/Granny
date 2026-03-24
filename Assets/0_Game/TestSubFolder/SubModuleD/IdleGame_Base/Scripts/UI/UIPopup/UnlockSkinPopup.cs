using Cysharp.Text;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if FIREBASE_AVAILABLE
using Firebase.Analytics;
#endif

public class UnlockSkinPopup : BasePopup,ISkinControllerUser
{
    public Image m_image;
    public TMP_Text m_name;
    public TMP_Text m_moneyPrice;
    public TMP_Text m_adAmount;

    public ButtonEffectLogic m_buyButton;
    public ButtonEffectLogic m_cancelButton;
    public MoneyController m_moneyController;

    public SkinShopData m_currentSkinShopData;
    //public SkinShopListDisplayPopup m_skinShop;
    private SkinPurchasableZone m_currentSkinAdAmount;

    public GameObject m_adWatch;
    public GameObject m_moneyBuy;
    public TAnimCore m_textAnim;

    private bool m_useMoney;

    public SkinController BloxSkinController { get; set; }

    protected override void Awake()
    {
        base.Awake();
        m_cancelButton.onClick.AddListener(Hide);
        //m_skinShop = transform.parent.GetComponentInChildren<SkinShopListDisplayPopup>(true);

        if (Utils_UIController.on_off_reward_button)
        {
            m_buyButton.onClick.AddListener(WatchAds);
        }
        else
        {
            m_buyButton.gameObject.SetActive(false);
        }
        
        
    }
    
    void WatchAds()
    {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
            Buy();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_buy_skin);
#elif NO_ADS
            Buy();
#else
            Consts.NotiNoAds();
#endif
    }
    
    
    void Buy()
    {
        if (m_useMoney && !m_moneyController.AttemptPurchase(m_currentSkinShopData.m_itemPrice)) return;
        m_currentSkinAdAmount.OnIncrease?.Invoke(m_currentSkinAdAmount);
        UpdateMoney();

        if (BloxSkinController.IsSkinUnlocked(m_currentSkinShopData))
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase(Consts.rw_buy_skin_in_map, new Parameter[]
            {
                new Parameter(Consts.SkinID, m_currentSkinAdAmount.ItemId)
            });
#endif
            Hide();
        }
    }

    public void SetSkinData(SkinPurchasableZone skinAdAmount)
    {
  
        int id = skinAdAmount.ItemId;
        m_currentSkinShopData = BloxSkinController.m_skinData.m_shopItemData[id];
        m_currentSkinAdAmount = skinAdAmount;
        m_image.sprite = m_currentSkinShopData.m_displayIcon;

       
        
        m_useMoney = m_currentSkinShopData.m_itemPrice > 0;

        if (m_useMoney && m_currentSkinShopData.m_itemPrice > 100000 || m_currentSkinShopData.m_itemAdAmount >= 3)
        {
            m_textAnim.SetText(ZString.Format("<rainb>{0}</rainb>", m_currentSkinShopData.m_itemName));
            //m_name.SetTextFormat("<rainb>{0}</rainb>", m_currentSkinShopData.m_itemName);
        }
        m_name.SetText(m_currentSkinShopData.m_itemName);
        m_adWatch.SetActive(!m_useMoney);
        m_moneyBuy.SetActive(m_useMoney);
        UpdateMoney();
        
    }
    void UpdateMoney()
    {
        if (!m_useMoney) SetAdText(m_currentSkinAdAmount.m_currentCnt, m_currentSkinAdAmount.m_maxCnt);
        else SetMoneyText();
    }
    void SetAdText(int a, int b)
    {
        m_adAmount.SetTextFormat("GET {0}/{1}", a, b);
    }
    void SetMoneyText()
    {
        MoneyUIView.UpdateMoney(m_currentSkinShopData.m_itemPrice, m_moneyPrice);
    }
    
    public override void Hide()
    {
        base.Hide();
#if ADS_AVAILABLE
        AdsAdapter.Instance.HideMrec();
        AdsAdapter.Instance.ShowInterstitial(Mediation_Manager.GameID(), AdsAdapter.@where.inter_close_popup);
#endif
  
    }
    public override void Show()
    {
        base.Show();
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowMrec();
#endif
    }
}
