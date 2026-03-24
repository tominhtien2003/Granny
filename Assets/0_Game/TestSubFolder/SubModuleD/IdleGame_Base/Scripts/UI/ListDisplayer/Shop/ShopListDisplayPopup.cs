using Cysharp.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public abstract class ShopListDisplayPopup : ListDisplayPopup
{
    public TMP_Text m_price;
    public TMP_Text m_name;
    public PurchasableItemButton m_buttonPrefab;

    public ButtonEffectLogic m_selectButton;

    public GameObject m_selectedGraphic;
    public GameObject m_selectGraphic;
    public GameObject m_purchaseGraphic;

    protected ShopItem m_currentDisplay;
    protected ShopItem m_currentSelected;
    protected PurchasableItemButton m_currentButton;
    private bool m_unlocked;
    private bool m_selected;
    protected Febucci.UI.Core.TAnimCore m_textAnim;

    public override void InitButtons()
    {
        m_selectButton.onClick.AddListener(CurrentItemAction);
    }
    void CurrentItemAction()
    {
        if (m_selected) return;
        if (m_unlocked)
        {
            this.Log("Selecting...");
            
            m_currentSelected = m_currentDisplay;
            BuyItem(m_currentDisplay);
            
        }

        else
        {
            this.Log("Buying...");
            if (m_currentDisplay.m_itemPrice == 0 || HeightMoneyController.Instance.AttemptPurchase(m_currentDisplay.m_itemPrice))
            {
                AttemptBuyItem();
            }
                         
        }
    }
    protected virtual void AttemptBuyItem()
    {
        m_currentButton.OnIncrease?.Invoke(m_currentButton);
        OnItemBuying();
    }
    protected virtual void OnItemBuying()
    {

    }
    public virtual void BuyItem(ShopItem item)
    {
        m_currentDisplay = item;
        SetCurrentItem();
        AssignItem(item);
        
        UpdateUnlockedStatus(item);
    }
    public override void DisplayData(DisplayItem item)
    {
        
        if (item == null) return;
        ShopItem shopItem = (item as ShopItem);
        if (m_currentDisplay == shopItem) return;
        base.DisplayData(item);

        MoneyUIView.UpdateMoney(shopItem.m_itemPrice, m_price);
        //m_price.SetTextFormat("{0}", shopItem.m_itemPrice);
        m_currentDisplay = shopItem;
        UpdateUnlockedStatus(shopItem);
        //m_name.SetText(shopItem.m_itemName);

    }

    void UpdateUnlockedStatus(ShopItem shopItem)
    {
//        this.Log("updating unlocked...");
        m_unlocked = IsItemUnlocked(shopItem);
        m_selected = IsItemSelected(shopItem);

        m_selectedGraphic.SetActive(m_unlocked & m_selected);
        m_selectGraphic.SetActive(m_unlocked & !m_selected);
        m_purchaseGraphic.SetActive(!m_unlocked);

        
    }
    protected abstract void SetCurrentItem();

    protected abstract bool IsItemUnlocked(ShopItem shopItem);
    protected abstract bool IsItemSelected(ShopItem shopItem);

    protected abstract void AssignItem(ShopItem shopItem);
    public override void InitList()
    {
        m_textAnim = m_name.GetComponent<Febucci.UI.Core.TAnimCore>();
    }
    protected virtual void SetupButton(PurchasableItemButton button, ShopItem shopItem, int id)
    {
        button.InitButton(shopItem);
        button.InitId(id, IsItemUnlocked(shopItem));


        
        button.OnShopClickAction += (PurchasableItemButton butt) => m_currentButton = butt;
        button.OnClickAction += DisplayData;
        button.OnItemBoughtAction += (ShopItem shopItem) =>
        {
            /*            if (m_currentDisplay == shopItem)
                        {
                            this.Log(shopItem + " unlocked");*/
            /*m_currentSelected = shopItem;
            UpdateUnlockedStatus(shopItem);
*/
            ParticleManager.Instance.PlayConfetti();
            //m_currentDisplay = shopItem;
            if (m_currentDisplay != shopItem)
            {
                DisplayData(shopItem);
            }
            SetCurrentItem();
            UpdateUnlockedStatus(m_currentDisplay);

            SetCurrentItem();
            AssignItem(m_currentDisplay);
            
                
            //}
        };
    }
#if UNITY_EDITOR

    [Button]
    public void ShopListINIT()
    {
        m_previewImage = Utils.FindInChildren(gameObject, "PreviewImage").GetComponent<Image>();
        m_contentTransform = Utils.FindInChildren(gameObject, "Content").transform;
        m_price = Utils.FindInChildren(gameObject, "PriceText").GetComponent<TextMeshProUGUI>();
        m_selectButton = Utils.FindInChildren(gameObject, "Buy Button").GetComponent<ButtonEffectLogic>();
        m_selectedGraphic = Utils.FindInChildren(gameObject, "Selected");
        m_selectGraphic = Utils.FindInChildren(gameObject, "Bought");
        m_purchaseGraphic = Utils.FindInChildren(gameObject, "Buy");

        this.SetDirty();
    }

#endif
}
