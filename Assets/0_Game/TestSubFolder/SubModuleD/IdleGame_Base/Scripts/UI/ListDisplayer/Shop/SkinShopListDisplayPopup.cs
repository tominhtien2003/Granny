using Cysharp.Text;
#if FIREBASE_AVAILABLE
using Firebase.Analytics;
#endif
using TMPro;
using UnityEngine;

public class SkinShopListDisplayPopup : ShopListDisplayPopup,ISkinControllerUser
{
    private SO_ShopData m_shopItemDataSO;
    public GameObject m_moneyBuy;
    public GameObject m_adWatch;
    public TMP_Text m_adAmount;
    public SkinController BloxSkinController { get; set; }

    protected override void SetCurrentItem()
    {
        BloxSkinController.SetCurrentSkin(m_currentDisplay);
    }
    protected override bool IsItemUnlocked(ShopItem shopItem)
    {
        return BloxSkinController.IsSkinUnlocked(shopItem);
    }
    protected override void AttemptBuyItem()
    {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
            base.AttemptBuyItem();
            if (BloxSkinController.IsSkinUnlocked(m_currentDisplay))
            {
#if FIREBASE_AVAILABLE
                TrackingEvent.LogFirebase(Consts.rw_buy_skin_in_shop, new Parameter[]
                {
                    new Parameter(Consts.SkinID, m_currentButton.ItemId.ToString())
                });
#endif
            }
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_buy_skin);
#elif NO_ADS
		base.AttemptBuyItem();
#else
		Consts.NotiNoAds();
#endif
    }
    protected override void AssignItem(ShopItem shopItem)
    {
        if (PlayerSkinDataSetter.Instance == null) return;
        PlayerSkinDataSetter.Instance.AssignPlayerModel(shopItem as SkinShopData);
    }
    public override void InitList()
    {
        base.InitList();
        m_shopItemDataSO = BloxSkinController.m_skinData;
        for (int i = 0; i < m_shopItemDataSO.m_shopItemData.Count; i++)
        {
            ShopItem shopItem = m_shopItemDataSO.m_shopItemData[i];
            PurchasableItemButton button = Instantiate(m_buttonPrefab, m_contentTransform);

            SetupButton(button, shopItem, i);
            BloxSkinController.AssignSkinAdAmount(button, shopItem.m_itemPrice > 0);
        }
        DisplayData(m_shopItemDataSO.m_shopItemData[BloxSkinController.UserCurrentSkin]);
    }
    void SetAdText(int a, int b)
    {
        m_adAmount.SetTextFormat("GET {0}/{1}", a, b);
    }
    private bool m_useMoney;
    public override void DisplayData(DisplayItem item)
    {
        base.DisplayData(item);
        m_useMoney = m_currentDisplay.m_itemPrice > 0;
        if ((m_useMoney && m_currentDisplay.m_itemPrice > 100000) ||
        m_currentDisplay.m_itemAdAmount >= 4)
        {
            //m_name.SetTextFormat("<rainb>{0}</rainb>", m_currentDisplay.m_itemName);
            m_textAnim.SetText(ZString.Format("<rainb>{0}</rainb>", m_currentDisplay.m_itemName));
        }
        else
        {
            m_name.color = WingShopListDisplayPopup.m_colrs[Mathf.Max(0,m_currentDisplay.m_itemAdAmount - 1)];
            m_name.SetText(m_currentDisplay.m_itemName);
        }

        if (IsItemUnlocked(m_currentDisplay))
        {
            if (!Utils_UIController.on_off_reward_button)
            {
                m_selectButton.gameObject.SetActive(true);
            }
            return;
        };
        
        m_moneyBuy.SetActive(m_useMoney);
        if (Utils_UIController.on_off_reward_button)
        {
            m_adWatch.SetActive(!m_useMoney);
        }
        else
        {
            if(!m_useMoney)  m_selectButton.gameObject.SetActive(false);
        }

        if (!m_useMoney)
        {
            if (m_currentButton == null) this.LogError("No button to display");
            else SetAdText(m_currentButton.m_currentAmount, m_currentButton.m_maxAmount);
        }
    }
    protected override bool IsItemSelected(ShopItem shopItem)
    {
        return BloxSkinController.IsSkinSelected(shopItem);
    }
    protected override void OnItemBuying()
    {
        base.OnItemBuying();
        if (!m_useMoney) SetAdText(m_currentButton.m_currentAmount, m_currentButton.m_maxAmount);
    }

    public override void Hide()
    {
        base.Hide();
 #if ADS_AVAILABLE
        AdsAdapter.Instance.ShowInterstitial(Mediation_Manager.GameID(), AdsAdapter.@where.inter_close_popup);
#endif
    }
}
