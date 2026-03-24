using UnityEngine;

public class CoinPurchaseAddon : MonoBehaviour
{
    public MoneyController m_coin;
    public MoneyController m_gem;
    public MoneyController m_cup;

    public FreePopup m_freePopup;
    public string NOT_ENOUGH_CUP = "Not enough cup! Climb to the top of the tower to receive cup!";
    private void Awake()
    {
        if(m_coin) m_coin.OnPurchaseFailure += NotEnoughCoins;
        if(m_gem) m_gem.OnPurchaseFailure += NotEnoughGems;
        if(m_cup) m_cup.OnPurchaseFailure += NotEnoughCups;
    }

    void NotEnoughCoins()
    {
        UINotify.Instance.NotifyMoney();
        if (m_freePopup)
        {
            m_freePopup.SetupOpen(0);
            m_freePopup.Show();
        }
        else
        {
#if UNITY_EDITOR
            this.LogError("k co freePopup");
#endif
        }
    }
    void NotEnoughGems()
    {
        UINotify.Instance.NotifyGem();
        if (m_freePopup)
        {
            m_freePopup.SetupOpen(1);
            m_freePopup.Show();
        }
        else
        {
    #if UNITY_EDITOR
            this.LogError("k co freePopup");
    #endif
        }
    }
    void NotEnoughCups()
    {
        UINotify.Instance.Notify(NOT_ENOUGH_CUP);
      
    }
}
