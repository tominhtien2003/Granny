using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAdZone : BasePurchasableZone, IAdAmount
{
    public TextMeshPro m_adAmountText;

    protected string nameAdZone;

    public virtual string ItemName { get; set; }

    public virtual int ItemId { get; set; }

    public UnityAction<IAdAmount> OnIncrease
    {
        get => m_onIncrease;
        set => m_onIncrease = value;
    }

    public UnityAction<IAdAmount> OnReset
    {
        get => m_onReset;
        set => m_onReset = value;
    }

    private bool m_hasAdIcon;

    private UnityAction<IAdAmount> m_onIncrease;
    private UnityAction<IAdAmount> m_onReset;
    public List<GameObject> m_ringParticles;
    private int m_currentRing = -1;

    protected virtual void SetRing(int id)
    {
        if (m_currentRing == id) return;
        if (m_currentRing >= 0)
        {
            m_ringParticles[m_currentRing].SetActive(false);
        }

        m_ringParticles[id].SetActive(true);
        m_currentRing = id;
    }

    protected override void UnlockDone()
    {
        base.UnlockDone();
        ShowAd();
    }

    void ShowAd()
    {
        /*AdsAdapter.Instance.ShowRewardedVideo(DataController.UserRebirth, TryUnlock, () =>
        {
            Consts.NotiAdsFail();
        }, AdsAdapter.@where.rw_brainrot_item_in_map);*/
        TryUnlock();
    }

    protected virtual void TryUnlock()
    {
        OnIncrease?.Invoke(this);
    }

    protected virtual void Unlocked()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnUpdateAdAmount(int amount, int maxAmount)
    {
        /* m_adAmountText.text = $"{amount} / {maxAmount}";
         if (amount >= maxAmount)
         {
             //if(Player.Instance) Player.Instance.BuySuccessParticle();
             Unlocked();
         }*/
        if (amount >= maxAmount) Unlocked();
    }

    public UnityAction<IAdAmount> OnDestroyed
    {
        get => m_onDestroy;
        set => m_onDestroy = value;
    }

    private UnityAction<IAdAmount> m_onDestroy;

    void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}