using UnityEngine;

public class SlideReceiveAddon : MonoBehaviour
{
    public SlideDown m_slideState;
    private ReceiveMoneyScript m_moneyReceive;
    private void Awake()
    {
        m_moneyReceive = GetComponent<ReceiveMoneyScript>();
        m_slideState.OnStateChanged += FallEvent;
    }
    void FallEvent(bool state)
    {
        if (state) return;
        m_moneyReceive.FallEvent();
    }
}
