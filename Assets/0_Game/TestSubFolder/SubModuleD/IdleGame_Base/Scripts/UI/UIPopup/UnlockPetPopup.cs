using Cysharp.Text;
using Febucci.UI.Core;
using TMPro;
using UnityEngine.UI;
#if FIREBASE_AVAILABLE
using Firebase.Analytics;
#endif

public class UnlockPetPopup : BasePopup
{
    public Image m_petImage;
    public TMP_Text m_petName;
    public TMP_Text m_petPrice;
    public TMP_Text m_petAdAmount;

    public ButtonEffectLogic m_buyWithGemButton;
    public ButtonEffectLogic m_buyWithAdsButton;
    public ButtonEffectLogic m_cancelButton;
    public MoneyController m_gemMoneyController;

    public PetItem m_petData;
    public HatchEggPopup m_hatchPopup;
    //public Floor_HatchEggPopup floorHatchPopup;
    private PetPurchasableZone m_petZone;

    private int m_currentAds = 0;
    public TAnimCore m_textAnim;
    protected override void Awake()
    {
        base.Awake();
        m_buyWithGemButton.onClick.AddListener(Buy);
        m_cancelButton.onClick.AddListener(Hide);
        if (Utils_UIController.on_off_reward_button)
        {
            m_buyWithAdsButton.onClick.AddListener(WatchAds);
        }
        else
        {
            m_buyWithAdsButton.gameObject.SetActive(false);
        }
    }
    
    void WatchAds()
    {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
            TryUnlock();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_buy_pet);
#elif NO_ADS
        TryUnlock();
#else
        Consts.NotiNoAds();
#endif
    }
    
    
    private void TryUnlock()
    {
        m_petZone.m_currentAdAmount++;
        if (m_petZone.m_currentAdAmount >= m_petZone.m_totalAdAmount)
        {
            
            if (m_hatchPopup)
            {
#if FIREBASE_AVAILABLE
                TrackingEvent.LogFirebase(Consts.rw_buy_pet, new Parameter[]
                {
                    new Parameter(Consts.PetID,  m_petZone.ItemId)
                });
#endif
                m_hatchPopup.ReceivePet(m_petZone.ItemId, this);
            }
            /*else if (floorHatchPopup)
            {
                //TrackingEvent.LogFirebase($"rw_floor_buy_pet_{m_petData.m_id}", null);
                TrackingEvent.LogFirebase($"rw_buy_pet_game_{GameManager.MiniGameIndex}", new Parameter[]
                {
                    new Parameter(Consts.PetID, m_petData.m_id)
                });
                floorHatchPopup.ReceivePet(m_petZone.ItemId, this);
            }*/
            m_petZone.m_currentAdAmount = 0;
            Hide();
        }
        SetAdText();
    }
    
    void Buy()
    {
        if (m_hatchPopup)
        {
            if (m_hatchPopup.m_inventory.IsFull() || !m_gemMoneyController.AttemptPurchase(m_petData.m_gemPrice)) return;
        
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase(Consts.buy_pet_with_gem, new Parameter[]
            {
                new Parameter(Consts.PetID, m_petZone.ItemId)
            });
#endif
            m_hatchPopup.ReceivePet(m_petZone.ItemId, this);
            Hide();
        }
        /*else
        {
            if (floorHatchPopup.m_inventory.IsFull() || !m_gemMoneyController.AttemptPurchase(m_petData.m_gemPrice)) return;
        
            TrackingEvent.LogFirebase($"buy_pet_with_gems_game_{GameManager.MiniGameIndex}", new Parameter[]
            {
                new Parameter(Consts.PetID, m_petData.m_id)
            });
            //TrackingEvent.LogFirebase($"floor_buy_pet_with_gems_{m_petData.m_id}", null);
            floorHatchPopup.ReceivePet(m_petZone.ItemId, this);
            Hide();
        }*/
      
    }

    public void SetPetData(PetPurchasableZone petzone)
    {
        int id = petzone.ItemId;
        m_petData = ClimbAndJump_DataController.Instance.m_petData.m_petItems[id];
        m_petZone = petzone;
        m_petImage.sprite = m_petData.m_displayIcon;
        if (m_petData.m_rarityIndex == 4)
        {
            m_textAnim.SetText(ZString.Format("<rainb>{0}</rainb>", m_petData.m_itemName));
            //m_petName.SetTextFormat("<rainb>{0}</rainb>", m_petData.m_itemName);
        }
        else m_petName.SetText(m_petData.m_itemName);
        m_petPrice.SetTextFormat("{0}", m_petData.m_gemPrice);
       
        SetAdText();
    }
    void SetAdText()
    {
        m_petAdAmount.SetTextFormat("GET {0}/{1}", m_petZone.m_currentAdAmount, m_petZone.m_totalAdAmount);
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
