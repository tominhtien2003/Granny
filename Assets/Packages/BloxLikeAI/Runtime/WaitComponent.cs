public class WaitComponent
{
    public bool IsWaiting;
    float m_currentWaitDuration;
    public float CurrentWaitDuration => m_currentWaitDuration;
    public void StartWait(float waitTime)
    {
        IsWaiting = true;
        m_currentWaitDuration = waitTime;
    }
    public void UpdateWait(float dt)
    {
        if (IsWaiting)
        {
            m_currentWaitDuration -= dt;
            if (m_currentWaitDuration < 0)
            {
                IsWaiting = false;
            }
            return;
        }
    }
}
