using UnityEngine;
public interface IMinigameId
{
    public abstract int MinigameId { get; set; }
}
public class MoneySaveControl : MonoBehaviour, IMinigameId
{
    public MoneyController m_coinMoney;
    public MoneyController m_gemMoney;
    public MoneyController m_cupMoney;
    private int _minigameId = 0;
   
    public int MinigameId
    {
        get => _minigameId;
        set => _minigameId = value;
    }

    private void Awake()
    {
        if(m_cupMoney) m_cupMoney.UpdateSaveMoney += SaveCupMoney;
        if(m_coinMoney) m_coinMoney.UpdateSaveMoney += SaveCoinMoney;
        if(m_gemMoney) m_gemMoney.UpdateSaveMoney += SaveGemMoney;

        
    }
    private void Start()
    {
        if (m_cupMoney)
        {
            m_cupMoney.m_totalMoney = IdleGameBase_PrefData.GetCups();
            m_cupMoney.UpdateView(m_cupMoney.m_totalMoney);
        }

        if (m_coinMoney)
        {
            m_coinMoney.m_totalMoney = IdleGameBase_PrefData.GetCoins(m_coinMoney.m_defaultValue.ToString());
            m_coinMoney.UpdateView(m_coinMoney.m_totalMoney);
        }
        
        if (m_gemMoney)
        {
            m_gemMoney.m_totalMoney = IdleGameBase_PrefData.GetGems();
            m_gemMoney.UpdateView(m_gemMoney.m_totalMoney);
        }
    }
    void SaveCupMoney(MoneyController controller)
    {
        IdleGameBase_PrefData.SaveCups(m_cupMoney.m_totalMoney);
    }
    void SaveCoinMoney(MoneyController controller)
    {
        IdleGameBase_PrefData.SaveCoin(m_coinMoney.m_totalMoney);
    }
    void SaveGemMoney(MoneyController controller)
    {
        IdleGameBase_PrefData.SaveGems(m_gemMoney.m_totalMoney);
    }
}
