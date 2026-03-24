using UnityEngine;
using UnityEngine.Events;
public class PurchasableItemButton : ListDisplayItemButton, IAdAmount
{
    public string ItemName => "";

    public int ItemId => m_itemId;

    public UnityAction<IAdAmount> OnIncrease { get => m_onIncrease; set => m_onIncrease = value; }
    public UnityAction<IAdAmount> OnReset { get => m_onReset; set => m_onReset = value; }
    public UnityAction<IAdAmount> OnDestroyed { get => m_onDestroyed; set => m_onDestroyed = value; }


    private UnityAction<IAdAmount> m_onIncrease;
    private UnityAction<IAdAmount> m_onReset;
    private UnityAction<IAdAmount> m_onDestroyed;

    private int m_itemId;

    public GameObject m_lockedGraphics;
    public bool m_isPurchased;

    public UnityAction<PurchasableItemButton> OnShopClickAction;
    public UnityAction<ShopItem> OnItemBoughtAction;

    private ShopItem m_shopItemData;

    public int m_currentAmount;
    public int m_maxAmount;
    public void InitButton(ShopItem item)
    {
        m_selectButton.onClick.AddListener(() => OnShopClickAction(this));
        base.InitButton(item);
        m_shopItemData = item;
       
    }
    public void OnUpdateAdAmount(int amount, int maxAmount)
    {
        if (m_isPurchased) return;
        m_currentAmount = amount;
        m_maxAmount = maxAmount;
        //this.LogError("unlocked at " + m_itemId);
        if (amount >= maxAmount)
        {
            m_isPurchased = true;
            m_lockedGraphics.SetActive(!m_isPurchased);
            OnItemBoughtAction?.Invoke(m_shopItemData);
            
        }
    }
    public void InitId(int id, bool unlocked)
    {
        m_itemId = id;
        m_isPurchased = unlocked;
        m_lockedGraphics.SetActive(!m_isPurchased);
    }
    void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
