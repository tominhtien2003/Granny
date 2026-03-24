using UnityEngine;
using UnityEngine.UIElements;

public class G3_BotHealth : MonoBehaviour
{
    public System.Action OnDeath;
    public void Start()
    {
        
    }
    public void DieInstant()
    {
        OnDeath?.Invoke();
        G3_Manager.Instance.UpdateBotDestroyed();
        G3_BotSpawner.Instance.RemoveBot(this);
        G3_PoolManager.Instance.ReturnBot(this);
    }
}
