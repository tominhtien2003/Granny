using UnityEngine;
using UnityEngine.Events;
public interface IPetMultiplier
{
    public void ChangeMultiplier(double multiplier);
}
public class MoneyController : MonoBehaviour, IPetMultiplier
{
    public MoneyUIView m_view;
    public double m_currentPotentialMoney = 0;
    public double m_moneyMultiplier;
    public double m_totalMoney = 0;
    public UnityAction<MoneyController> UpdateSaveMoney;
    public UnityAction OnPurchaseFailure;

    public int m_defaultValue = 0;
    public void CalculatePotentialMoney()
    {
        double baseMoney = 0;
        CalculateBaseMoney(ref baseMoney);

        double newMoney = m_moneyMultiplier * baseMoney;
        if (m_currentPotentialMoney == newMoney) return;
        SetPotentialMoney(newMoney);
    }
    public void AddPotentialMoney(bool setGraphic = true)
    {
        AddToTotalMoney(m_currentPotentialMoney, setGraphic);
    }
    public void ResetPotentialMoney()
    {
        SetPotentialMoney(0);
    }

    protected void SetPotentialMoney(double money)
    {
        m_currentPotentialMoney = money;
        //UpdateSaveMoney?.Invoke(this);
        m_view.OnUpdateJumpMoney(m_currentPotentialMoney);
    }

    public void ResetMoney()
    {
        m_totalMoney = 0;
        UpdateSaveMoney?.Invoke(this);
        m_view.OnUpdateTotalMoney(m_totalMoney);
    }
    
    public void  AddToTotalMoney(double money, bool setGraphic = true)
    {
        m_totalMoney += money;
        UpdateSaveMoney?.Invoke(this);
        if (setGraphic) m_view.OnUpdateTotalMoney(m_totalMoney);
    }
    public void UpdateView(double money)
    {
        m_view.OnUpdateTotalMoney(money);
    }
    public void SetMultiplier(double multiplier)
    {
        m_moneyMultiplier = multiplier;
        //m_view.OnUpdateMultiplier(m_moneyMultiplier);
    }

    public void ChangeMultiplier(double diff)
    {
        SetMultiplier(m_moneyMultiplier + diff);
    }
    protected virtual void CalculateBaseMoney(ref double baseMoney)
    {

    }

    public bool EnoughMoney(double price)
    {
        return m_totalMoney >= price;
    }
    
    public bool AttemptPurchase(double price)
    {
        if (m_totalMoney < price)
        {
            OnPurchaseFailure?.Invoke();
            return false;
        }
        AddToTotalMoney(-price);
        return true;
    }

    public void ResetMultiplier()
    {
        m_moneyMultiplier = 1;
    }
    
    #if CHEAT
    public void CheatMoney(string money)
    {
        if (!double.TryParse(money, out double res)) return;
        this.LogError("Set");
        AddToTotalMoney(res - m_totalMoney);
    }
    #endif
}
