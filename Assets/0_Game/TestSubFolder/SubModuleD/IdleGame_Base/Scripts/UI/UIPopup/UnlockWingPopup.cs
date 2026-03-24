using Cysharp.Text;
using Febucci.UI.Core;
//using Firebase.Analytics;
using TMPro;
using UnityEngine.UI;

public class UnlockWingPopup : BasePopup
{
    public Image m_wingImage;
    public TMP_Text m_wingName;
    public TMP_Text m_wingPrice;

    public ButtonEffectLogic m_buyButton;
    public ButtonEffectLogic m_cancelButton;
    public MoneyController m_gemMoneyController;

    public WingDisplayData m_currentWingData;
    public WingShopListDisplayPopup m_wingShop;
    private IAdAmount m_currentWingAdAmount;
    public TAnimCore m_textAnim;

    protected override void Awake()
    {
        base.Awake();
        m_buyButton.onClick.AddListener(Buy);
        m_cancelButton.onClick.AddListener(Hide);
        m_wingShop = transform.parent.GetComponentInChildren<WingShopListDisplayPopup>(true);
    }
    void Buy()
    {
        if (!m_gemMoneyController.AttemptPurchase(m_currentWingData.m_gemPrice)) return;
        ClimbAndJump_DataController.BloxSkinController.m_wingDataHolder.AutoFillSkin(m_currentWingAdAmount.ItemId);
        //m_currentWingAdAmount.OnIncrease?.Invoke(m_currentWingAdAmount);
        //PlayerSkinDataSetter.Instance.AssignPlayerWings(m_currentWingData);
        //m_wingShop.DisplayData(m_currentWingData);
        //m_wingShop.BuyItem(m_currentWingData);
        //TrackingEvent.LogFirebase($"climb_buy_{(GameManager.MiniGameIndex == 0 ? Consts.wing : Consts.boat)}_with_gems_{m_currentWingAdAmount.ItemId}", null);
       /* TrackingEvent.LogFirebase($"buy_item_with_gems_game_{GameManager.MiniGameIndex}", new Parameter[]
        {
            new Parameter(Consts.ItemID, m_currentWingAdAmount.ItemId)
        });*/
        Hide();
    }
    public void SetWingData(IAdAmount wingAdAmount)
    {
        int id = wingAdAmount.ItemId;
        m_currentWingData = ClimbAndJump_DataController.Instance.m_wingShopData.m_wingShopItemData[id];
        m_currentWingAdAmount = wingAdAmount;
        m_wingImage.sprite = m_currentWingData.m_displayIcon;
        if (id > 15)
        {
            //m_wingName.SetTextFormat("<rainb>{0}</rainb>", m_currentWingData.m_itemName);
            m_textAnim.SetText(ZString.Format("<rainb>{0}</rainb>", m_currentWingData.m_itemName));
        }
        else m_wingName.SetText(m_currentWingData.m_itemName);
        
        m_wingPrice.SetTextFormat("{0}", m_currentWingData.m_gemPrice);
    }
}
