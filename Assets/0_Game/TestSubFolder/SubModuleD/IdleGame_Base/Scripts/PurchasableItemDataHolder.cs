using System;
using System.Collections.Generic;
using UnityEngine.Events;
public interface IAdAmount
{
    public string ItemName { get; }
    public int ItemId { get; }
    public UnityAction<IAdAmount> OnIncrease { get; set; }
    public UnityAction<IAdAmount> OnReset { get; set; }
    public UnityAction<IAdAmount> OnDestroyed { get; set; }
    public abstract void OnUpdateAdAmount(int amount, int maxAmount);
}
public class BaseItemAdStats
{
    public int Count;
    public int MaxCount;
    public List<IAdAmount> m_items;
    public BaseItemAdStats(int amount, int maxAmount)
    {
        Count = amount;
        MaxCount = maxAmount;
        m_items = new List<IAdAmount>();

    }
}
public class SkinItemAdStats : BaseItemAdStats
{
    public SkinItemAdStats(int amount, int maxAmount) : base(amount, maxAmount)
    {
    }
}
public class PurchasableItemDataHolder
{
    protected List<SkinItemAdStats> m_items = new List<SkinItemAdStats>();
    public Action<int> OnAmountFullAction;
    public void ClearAllItems()
    {
        foreach (var sk in m_items)
        {
            sk.m_items.Clear();
            sk.Count = 0;
        }

    }
    public void AddInit(SkinItemAdStats item)
    {
        m_items.Add(item);
    }
    public void AddItem(IAdAmount adAmount, bool useMoney = false)
    {
        m_items[adAmount.ItemId].m_items.Add(adAmount);
        if (!useMoney) adAmount.OnIncrease += UpdateIncrease;
        else adAmount.OnIncrease += UpdateIncreaseMoney;
        adAmount.OnDestroyed += OnItemDestroyed;
        //if (!AlreadyUnlocked(adAmount.ItemId)) 
        adAmount.OnUpdateAdAmount(m_items[adAmount.ItemId].Count, m_items[adAmount.ItemId].MaxCount);
    }

    void OnItemDestroyed(IAdAmount adAmount)
    {
        m_items[adAmount.ItemId].m_items.Remove(adAmount);
        adAmount.OnIncrease -= UpdateIncrease;
        adAmount.OnIncrease -= UpdateIncreaseMoney;
        adAmount.OnDestroyed -= OnItemDestroyed;
    }
    protected virtual bool AlreadyUnlocked(int id)
    {
        return true;
    }
    public SkinItemAdStats GetItem(IAdAmount adAmount)
    {
        return m_items[adAmount.ItemId];
    }
    protected virtual void UpdateIncreaseMoney(IAdAmount adAmount)
    {
        if (m_items[adAmount.ItemId].Count >= m_items[adAmount.ItemId].MaxCount) return;
        m_items[adAmount.ItemId].Count = m_items[adAmount.ItemId].MaxCount;
        OnAmountFull(adAmount);
        UpdateAmountSkin(adAmount);
    }
    protected virtual void UpdateIncrease(IAdAmount adAmount)
    {
        if (m_items[adAmount.ItemId].Count > m_items[adAmount.ItemId].MaxCount) return;
        m_items[adAmount.ItemId].Count++;

        if (m_items[adAmount.ItemId].Count >= m_items[adAmount.ItemId].MaxCount)
        {
            OnAmountFull(adAmount);
        }
        UpdateAmountSkin(adAmount);
    }
    protected void UpdateAmountSkin(IAdAmount adAmount)
    {
        foreach (IAdAmount aam in m_items[adAmount.ItemId].m_items)
        {
            aam.OnUpdateAdAmount(m_items[adAmount.ItemId].Count, m_items[adAmount.ItemId].MaxCount);
        }
    }

    protected virtual void OnAmountFull(IAdAmount adAmount)
    {
        OnAmountFullAction?.Invoke(adAmount.ItemId);
    }
    public void AutoFillSkin(int id)
    {
        m_items[id].Count = m_items[id].MaxCount;

        foreach (IAdAmount aam in m_items[id].m_items)
        {
            OnAmountFull(aam);
            aam.OnUpdateAdAmount(m_items[id].Count, m_items[id].MaxCount);
        }
    }
}
public class SkinItemDataHolder : PurchasableItemDataHolder
{
    public SkinController BloxSkinController;
    protected override void OnAmountFull(IAdAmount adAmount)
    {
        BloxSkinController.SetSkinUnlock(adAmount.ItemId);
        ParticleManager.Instance.PlayConfetti();
        base.OnAmountFull(adAmount);

    }

    protected override void UpdateIncreaseMoney(IAdAmount adAmount)
    {
        if (m_items[adAmount.ItemId].Count >= m_items[adAmount.ItemId].MaxCount) return;
        //if(adAmount is PurchasableItemButton) TrackingEvent.LogFirebase($"climb_buy_skin_with_money_{adAmount.ItemId}", null);
        base.UpdateIncreaseMoney(adAmount);
    }


}
public class WingItemDataHolder : PurchasableItemDataHolder
{
    public SkinController BloxSkinController;
    
    
    protected override void OnAmountFull(IAdAmount adAmount)
    {
        BloxSkinController.SetWingUnlock(adAmount.ItemId);
        ParticleManager.Instance.PlayConfetti();
        base.OnAmountFull(adAmount);

    }

    protected override void UpdateIncreaseMoney(IAdAmount adAmount)
    {
        if (m_items[adAmount.ItemId].Count >= m_items[adAmount.ItemId].MaxCount) return;
       base.UpdateIncreaseMoney(adAmount);
    }
    
    // protected override void OnAmountFull(IAdAmount adAmount)
    // {
    //     ClimbAndJump_DataController.Instance.SetWingUnlock(adAmount.ItemId);
    //     ParticleManager.Instance.PlayConfetti();
    //     base.OnAmountFull(adAmount);
    //
    // }
    //
    // protected override void UpdateIncreaseMoney(IAdAmount adAmount)
    // {
    //     if (m_items[adAmount.ItemId].Count >= m_items[adAmount.ItemId].MaxCount) return;
    //     if (adAmount is PurchasableItemButton)
    //     {
    //         //TrackingEvent.LogFirebase($"climb_buy_{(GameManager.MiniGameIndex == 0 ? Consts.wing : Consts.boat)}_with_money_{adAmount.ItemId}", null);
    //         /*TrackingEvent.LogFirebase($"buy_item_with_money_game_{GameManager.MiniGameIndex}", new Parameter[]
    //         {
    //             new Parameter(Consts.ItemID, adAmount.ItemId)
    //         });*/
    //         base.UpdateIncreaseMoney(adAmount);
    //     }
    // }
    
    public void AutoFillWing(int id)
    {
        m_items[id].Count = m_items[id].MaxCount;

        foreach (IAdAmount aam in m_items[id].m_items)
        {
            aam.OnUpdateAdAmount(m_items[id].Count, m_items[id].MaxCount);
        }
    }
}
