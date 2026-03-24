
public class TeleportPurchasableZone : BasePurchasableZone
{
    public TeleportPopup m_teleportPopup;
    protected override void UnlockDone()
    {
        base.UnlockDone();
        m_teleportPopup.Show();
    }
}
