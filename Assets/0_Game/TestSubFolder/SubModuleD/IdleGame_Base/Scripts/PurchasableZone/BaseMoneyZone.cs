public class BaseMoneyZone : BasePurchasableZone
{
    public long m_price;
    protected override void UnlockDone()
    {
        if (!HeightMoneyController.Instance.AttemptPurchase(m_price)) return;
        BuyItem();
    }

    protected virtual void BuyItem()
    {

    }
}
