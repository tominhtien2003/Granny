
using Cysharp.Text;
using TMPro;
using UnityEngine.Events;

public class EggPurchasableZone : BasePurchasableZone
{
    public HatchEggPopup m_hatchPopup;
    public int m_petData;
    public TMP_Text m_text;
    public UnityAction<int> OnUnlockDone;
    protected override void UnlockDone()
    {
        base.UnlockDone();
        OnUnlockDone?.Invoke(m_petData);
        /*m_hatchPopup.SetCurrentPetPool(m_petData);
        m_hatchPopup.Show();*/
    }
    public void SetPrice(long price)
    {
        MoneyUIView.UpdateMoney(price, m_text);
    }
}
